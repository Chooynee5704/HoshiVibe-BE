using HoshiVibe.DB;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace HoshiVibe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayOSController : ControllerBase
    {
        private readonly PayOSService _payOSService;
        private readonly DataContext _db;
        private readonly ILogger<PayOSController> _logger;
        private readonly HttpClient _httpClient;

        public PayOSController(PayOSService payOSService, DataContext db, ILogger<PayOSController> logger, IHttpClientFactory httpClientFactory)
        {
            _payOSService = payOSService;
            _db = db;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        public sealed class CreatePayOSReq
        {
            public required string OrderId { get; set; }
        }

        [HttpPost("create-payment-link")]
        public async Task<IActionResult> CreatePaymentLink([FromBody] CreatePayOSReq req, CancellationToken ct)
        {
            _logger.LogInformation($"=== PAYOS CREATE START === OrderId: {req.OrderId}");

            var order = await _db.Orders
                .FirstOrDefaultAsync(o => o.Order_Id == req.OrderId, ct);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            if (!string.Equals(order.Status, "Pending", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Order must be Pending");
            }

            try
            {
                var paymentData = _payOSService.CreatePaymentLink(order);
                
                // Create signature data string
                var signatureData = $"amount={paymentData.Amount}&cancelUrl={paymentData.CancelUrl}&description={paymentData.Description}&orderCode={paymentData.OrderCode}&returnUrl={paymentData.ReturnUrl}";
                var signature = CreateSignature(signatureData, paymentData.ChecksumKey);
                
                // Manual API call to PayOS
                var requestBody = new
                {
                    orderCode = paymentData.OrderCode,
                    amount = paymentData.Amount,
                    description = paymentData.Description,
                    cancelUrl = paymentData.CancelUrl,
                    returnUrl = paymentData.ReturnUrl,
                    signature = signature
                };

                var json = JsonSerializer.Serialize(requestBody);

                var request = new HttpRequestMessage(HttpMethod.Post, "https://api-merchant.payos.vn/v2/payment-requests")
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
                request.Headers.Add("x-client-id", paymentData.ClientId);
                request.Headers.Add("x-api-key", paymentData.ApiKey);

                var response = await _httpClient.SendAsync(request, ct);
                var responseContent = await response.Content.ReadAsStringAsync(ct);

                _logger.LogInformation($"PayOS Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
                    if (result.TryGetProperty("data", out var data) && data.ValueKind != JsonValueKind.Null)
                    {
                        var checkoutUrl = data.GetProperty("checkoutUrl").GetString();
                        return Ok(new { checkoutUrl, orderCode = paymentData.OrderCode });
                    }
                    else
                    {
                        return BadRequest(new { message = "PayOS returned null data", response = responseContent });
                    }
                }
                else
                {
                    return StatusCode((int)response.StatusCode, new { message = responseContent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating PayOS payment link");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook([FromBody] JsonElement webhookBody)
        {
            _logger.LogInformation("=== PAYOS WEBHOOK START ===");
            _logger.LogInformation($"Webhook Body: {webhookBody}");
            
            try
            {
                // Extract data from webhook
                var data = webhookBody.GetProperty("data");
                var orderCode = data.GetProperty("orderCode").GetInt64();
                var amount = data.GetProperty("amount").GetInt32();
                var description = data.GetProperty("description").GetString() ?? "";
                var status = data.GetProperty("status").GetString() ?? "";

                _logger.LogInformation($"OrderCode: {orderCode}, Amount: {amount}, Status: {status}");

                // Extract OrderId from description (format: "DH {orderId}")
                string orderId = description.Replace("DH ", "").Trim();

                var order = await _db.Orders.FirstOrDefaultAsync(o => o.Order_Id == orderId);
                if (order == null)
                {
                    _logger.LogError($"Order not found: {orderId}");
                    return Ok(new { code = "00", message = "Order not found" });
                }

                if (string.Equals(order.Status, "Paid", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation("Order already paid");
                    return Ok(new { code = "00", message = "Already paid" });
                }

                // Update order status based on PayOS status
                await using var tx = await _db.Database.BeginTransactionAsync();
                try
                {
                    if (status == "PAID")
                    {
                        order.Status = "Paid";
                        
                        _db.Payments.Add(new Payment
                        {
                            Order_Id = order.Order_Id,
                            Amount = amount,
                            PaymentDate = DateTime.UtcNow,
                            Status = "Success",
                            PaymentMethod = "PAYOS"
                        });
                    }
                    else if (status == "CANCELLED")
                    {
                        order.Status = "Cancelled";
                    }
                    else
                    {
                        order.Status = "Failed";
                    }

                    await _db.SaveChangesAsync();
                    await tx.CommitAsync();
                    
                    _logger.LogInformation($"Order updated to {order.Status}");
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    _logger.LogError(ex, "Error updating database");
                    return Ok(new { code = "00", message = "Database error" });
                }

                return Ok(new { code = "00", message = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling webhook");
                return Ok(new { code = "00", message = ex.Message });
            }
        }

        private string CreateSignature(string data, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
