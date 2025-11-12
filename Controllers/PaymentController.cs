using HoshiVibe.DB;
using HoshiVibe.Entities.DTO.ModelRequests.VNPay;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace HoshiVibe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly VnPayOption _opt;
        private readonly DataContext _db;
        private readonly ILogger<PaymentsController> _logger;
        private readonly VoucherService _voucherService;

        public PaymentsController(IOptions<VnPayOption> opt, DataContext db, ILogger<PaymentsController> logger, VoucherService voucherService)
        {
            _opt = opt.Value;
            _db = db;
            _logger = logger;
            _voucherService = voucherService;
        }

        private static string NowVnString()
        {
            var tzId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "SE Asia Standard Time"
                : "Asia/Ho_Chi_Minh";
            var tz = TimeZoneInfo.FindSystemTimeZoneById(tzId);
            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, tz).ToString("yyyyMMddHHmmss");
        }

        public sealed class CreateVnpayReq
        {
            public required string OrderId { get; set; }
            public string? BankCode { get; set; }
            public string? VoucherCode { get; set; }
        }

        [HttpPost("vnpay-create")]
        public async Task<IActionResult> Create([FromBody] CreateVnpayReq req, CancellationToken ct)
        {
            _logger.LogInformation("=== VNPAY CREATE START ===");
            _logger.LogInformation($"OrderId: {req.OrderId}, VoucherCode: {req.VoucherCode}");

            var order = await _db.Orders
                .FirstOrDefaultAsync(o => o.Order_Id == req.OrderId, ct);

            if (order == null)
            {
                _logger.LogError($"Order not found: {req.OrderId}");
                return NotFound("Order not found");
            }

            _logger.LogInformation($"Order Status: {order.Status}, FinalPrice: {order.FinalPrice}");

            if (!string.Equals(order.Status, "Pending", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning($"Order status is not Pending: {order.Status}");
                return BadRequest("Order must be Pending");
            }

            // Apply voucher discount if provided
            decimal finalAmount = order.FinalPrice;
            Guid? appliedVoucherId = null;

            if (!string.IsNullOrWhiteSpace(req.VoucherCode))
            {
                var voucher = await _voucherService.ValidateVoucherCode(req.VoucherCode);
                
                if (voucher != null)
                {
                    _logger.LogInformation($"Voucher valid: {voucher.Code}, Discount: {voucher.DiscountAmount}");
                    
                    // Apply discount
                    decimal discountAmount = order.FinalPrice * voucher.DiscountAmount;
                    finalAmount = order.FinalPrice - discountAmount;
                    appliedVoucherId = voucher.Voucher_Id;

                    // Update order with voucher
                    order.Voucher_Id = voucher.Voucher_Id;
                    order.FinalPrice = finalAmount;
                    
                    // Increment voucher used count
                    await _voucherService.UseVoucher(voucher.Voucher_Id);
                    
                    await _db.SaveChangesAsync(ct);
                    
                    _logger.LogInformation($"Discount applied: {discountAmount} VND, New FinalPrice: {finalAmount} VND");
                }
                else
                {
                    _logger.LogWarning($"Invalid voucher code: {req.VoucherCode}");
                    return BadRequest(new { message = "Mã giảm giá không hợp lệ hoặc đã hết hạn" });
                }
            }

            long amountVnd = (long)Math.Round(finalAmount, 0);
            var txnRef = $"{order.Order_Id}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
            var clientIp = HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "127.0.0.1";

            _logger.LogInformation($"Amount: {amountVnd} VND, TxnRef: {txnRef}");

            var vnp = new Dictionary<string, string>
            {
                ["vnp_Version"] = _opt.Version,
                ["vnp_Command"] = _opt.Command,
                ["vnp_TmnCode"] = _opt.TmnCode,
                ["vnp_Amount"] = (amountVnd * 100).ToString(),
                ["vnp_CurrCode"] = _opt.CurrCode,
                ["vnp_TxnRef"] = txnRef,
                ["vnp_OrderInfo"] = order.Order_Id,
                ["vnp_OrderType"] = "billpayment",
                ["vnp_Locale"] = _opt.Locale,
                ["vnp_ReturnUrl"] = _opt.ReturnUrl,
                ["vnp_IpAddr"] = clientIp,
                ["vnp_CreateDate"] = NowVnString()
            };

            if (!string.IsNullOrWhiteSpace(req.BankCode))
                vnp["vnp_BankCode"] = req.BankCode.Trim().ToUpperInvariant();

            var raw = HoshiVibe.Service.VnPayServiceSigner.BuildRawToSign(vnp);
            var sig = HoshiVibe.Service.VnPayServiceSigner.HmacSHA512(_opt.HashSecret, raw);
            vnp["vnp_SecureHash"] = sig;

            var qs = HoshiVibe.Service.VnPayServiceSigner.BuildEncodedQuery(vnp);
            var url = $"{_opt.PaymentUrl}?{qs}";

            _logger.LogInformation($"Payment URL created: {_opt.PaymentUrl}");
            _logger.LogInformation("=== VNPAY CREATE END ===");

            return Ok(new { paymentUrl = url, orderId = order.Order_Id, amountVnd, voucherApplied = appliedVoucherId != null });
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> Return(CancellationToken ct)
        {
            _logger.LogInformation("=== VNPAY RETURN START ===");

            var q = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());

            // Log tất cả params
            _logger.LogInformation("Query Parameters:");
            foreach (var kvp in q)
            {
                _logger.LogInformation($"  {kvp.Key} = {kvp.Value}");
            }

            // 1) Verify chữ ký
            if (!q.TryGetValue("vnp_SecureHash", out var sig))
            {
                _logger.LogError("Missing vnp_SecureHash");
                return Redirect($"{_opt.FrontendFailUrl}?error=missing_signature");
            }

            var filtered = q.Where(x => x.Key != "vnp_SecureHash" && x.Key != "vnp_SecureHashType")
                            .ToDictionary(x => x.Key, x => x.Value);
            var raw = HoshiVibe.Service.VnPayServiceSigner.BuildRawToSign(filtered);
            var calc = HoshiVibe.Service.VnPayServiceSigner.HmacSHA512(_opt.HashSecret, raw);
            var valid = sig.Equals(calc, StringComparison.OrdinalIgnoreCase);

            _logger.LogInformation($"Signature Valid: {valid}");
            _logger.LogInformation($"Received Signature: {sig}");
            _logger.LogInformation($"Calculated Signature: {calc}");

            var rsp = q.GetValueOrDefault("vnp_ResponseCode", "");
            var orderId = q.GetValueOrDefault("vnp_OrderInfo", "");
            var amountSt = q.GetValueOrDefault("vnp_Amount", "");

            _logger.LogInformation($"ResponseCode: {rsp}");
            _logger.LogInformation($"OrderId: {orderId}");
            _logger.LogInformation($"Amount: {amountSt}");

            if (!valid || string.IsNullOrWhiteSpace(orderId))
            {
                _logger.LogError("Invalid signature or missing orderId");
                return Redirect($"{_opt.FrontendFailUrl}?error=invalid_signature");
            }

            // 2) Load order
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Order_Id == orderId, ct);
            if (order == null)
            {
                _logger.LogError($"Order not found in DB: {orderId}");
                return Redirect($"{_opt.FrontendFailUrl}?error=order_not_found");
            }

            _logger.LogInformation($"Order found - Current Status: {order.Status}");

            long expected = (long)Math.Round(order.FinalPrice, 0);
            if (!long.TryParse(amountSt, out var got) || got / 100 != expected)
            {
                _logger.LogError($"Amount mismatch - Expected: {expected}, Got: {got / 100}");
                return Redirect($"{_opt.FrontendFailUrl}?error=amount_mismatch");
            }

            // 3) Idempotent check
            if (string.Equals(order.Status, "Paid", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("Order already paid - idempotent check");
                return Redirect($"{_opt.FrontendSuccessUrl}?orderId={order.Order_Id}&status=already_paid");
            }

            // 4) Update status
            _logger.LogInformation($"Starting transaction to update order status. ResponseCode: {rsp}");

            await using var tx = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                string newStatus = "";
                string paymentStatus = "";

                if (rsp == "00")
                {
                    newStatus = "Paid";
                    paymentStatus = "Success";
                    _logger.LogInformation("Setting status to Paid/Success");
                }
                else if (rsp == "24")
                {
                    newStatus = "Cancelled";
                    paymentStatus = "Cancelled";
                    _logger.LogInformation("Setting status to Cancelled");
                }
                else
                {
                    newStatus = "Failed";
                    paymentStatus = "Failed";
                    _logger.LogInformation($"Setting status to Failed (ResponseCode: {rsp})");
                }

                order.Status = newStatus;
                _logger.LogInformation($"Order.Status set to: {order.Status}");

                var payment = new Payment
                {
                    Order_Id = order.Order_Id,
                    Amount = expected,
                    PaymentDate = DateTime.UtcNow,
                    Status = paymentStatus,
                    PaymentMethod = "VNPAY"
                };

                _db.Payments.Add(payment);
                _logger.LogInformation("Payment record added to context");

                var savedChanges = await _db.SaveChangesAsync(ct);
                _logger.LogInformation($"SaveChanges affected {savedChanges} rows");

                await tx.CommitAsync(ct);
                _logger.LogInformation("Transaction committed successfully");

                // Verify update
                var verifyOrder = await _db.Orders.AsNoTracking()
                    .FirstOrDefaultAsync(o => o.Order_Id == orderId, ct);
                _logger.LogInformation($"Verification - Order Status in DB: {verifyOrder?.Status}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during transaction");
                _logger.LogError($"Exception Type: {ex.GetType().Name}");
                _logger.LogError($"Exception Message: {ex.Message}");
                _logger.LogError($"Stack Trace: {ex.StackTrace}");

                await tx.RollbackAsync(ct);
                return Redirect($"{_opt.FrontendFailUrl}?error=server_error");
            }

            var redirectUrl = (rsp == "00" && valid)
                ? $"{_opt.FrontendSuccessUrl}?orderId={orderId}&status=success&responseCode={rsp}"
                : $"{_opt.FrontendFailUrl}?orderId={orderId}&status=failed&responseCode={rsp}";

            _logger.LogInformation($"Redirecting to: {redirectUrl}");
            _logger.LogInformation("=== VNPAY RETURN END ===");

            return Redirect(redirectUrl);
        }

        [HttpGet("check-order-status/{orderId}")]
        public async Task<IActionResult> CheckOrderStatus(string orderId, CancellationToken ct)
        {
            _logger.LogInformation($"=== CHECK ORDER STATUS: {orderId} ===");

            var order = await _db.Orders
                .Include(o => o.Payment)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Order_Id == orderId, ct);

            if (order == null)
            {
                _logger.LogError($"Order not found: {orderId}");
                return NotFound(new { message = "Order not found" });
            }

            _logger.LogInformation($"Order Status: {order.Status}");
            _logger.LogInformation($"Payment: {(order.Payment != null ? "EXISTS" : "NULL")}");

            return Ok(new
            {
                orderId = order.Order_Id,
                status = order.Status,
                finalPrice = order.FinalPrice,
                payment = order.Payment != null ? new
                {
                    paymentId = order.Payment.Payment_Id,
                    amount = order.Payment.Amount,
                    status = order.Payment.Status,
                    paymentMethod = order.Payment.PaymentMethod,
                    paymentDate = order.Payment.PaymentDate
                } : null
            });
        }

        [HttpGet("vnpay-ipn")]
        public async Task<IActionResult> Ipn(CancellationToken ct)
        {
            _logger.LogInformation("=== VNPAY IPN START ===");

            var q = Request.Query.ToDictionary(k => k.Key, v => v.Value.ToString());

            foreach (var kvp in q)
            {
                _logger.LogInformation($"  {kvp.Key} = {kvp.Value}");
            }

            if (!q.TryGetValue("vnp_SecureHash", out var sig))
            {
                _logger.LogError("IPN: Missing signature");
                return Ok(new { RspCode = "97", Message = "Missing signature" });
            }

            var filtered = q.Where(x => x.Key != "vnp_SecureHash" && x.Key != "vnp_SecureHashType")
                            .ToDictionary(x => x.Key, x => x.Value);

            var raw = HoshiVibe.Service.VnPayServiceSigner.BuildRawToSign(filtered);
            var calc = HoshiVibe.Service.VnPayServiceSigner.HmacSHA512(_opt.HashSecret, raw);
            if (!sig.Equals(calc, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogError("IPN: Invalid signature");
                return Ok(new { RspCode = "97", Message = "Invalid signature" });
            }

            var orderId = q.GetValueOrDefault("vnp_OrderInfo", "");
            if (string.IsNullOrWhiteSpace(orderId))
            {
                _logger.LogError("IPN: Invalid order");
                return Ok(new { RspCode = "99", Message = "Invalid order" });
            }

            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Order_Id == orderId, ct);
            if (order == null)
            {
                _logger.LogError($"IPN: Order not found: {orderId}");
                return Ok(new { RspCode = "01", Message = "Order not found" });
            }

            long expected = (long)Math.Round(order.FinalPrice, 0);
            if (!long.TryParse(q.GetValueOrDefault("vnp_Amount"), out var got) || got / 100 != expected)
            {
                _logger.LogError("IPN: Invalid amount");
                return Ok(new { RspCode = "04", Message = "Invalid amount" });
            }

            if (string.Equals(order.Status, "Paid", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("IPN: Order already paid");
                return Ok(new { RspCode = "00", Message = "Already confirmed" });
            }

            var rsp = q.GetValueOrDefault("vnp_ResponseCode", "");
            var success = rsp == "00";

            _logger.LogInformation($"IPN: Updating order. ResponseCode: {rsp}");

            await using var tx = await _db.Database.BeginTransactionAsync(ct);
            try
            {
                order.Status = success ? "Paid" : (rsp == "24" ? "Cancelled" : "Failed");

                _db.Payments.Add(new Payment
                {
                    Order_Id = order.Order_Id,
                    Amount = expected,
                    PaymentDate = DateTime.UtcNow,
                    Status = success ? "Success" : (rsp == "24" ? "Cancelled" : "Failed"),
                    PaymentMethod = "VNPAY"
                });

                await _db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);

                _logger.LogInformation("IPN: Transaction committed");
                return Ok(new { RspCode = "00", Message = "Confirm Success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "IPN: Error during transaction");
                await tx.RollbackAsync(ct);
                return Ok(new { RspCode = "99", Message = "Server error" });
            }
        }
    }
}