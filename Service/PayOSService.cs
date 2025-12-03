using HoshiVibe.Entities.DTO.ModelRequests.PayOSModels;
using HoshiVibe.Entities.Models.Base;
using Microsoft.Extensions.Options;

namespace HoshiVibe.Service
{
    public class PayOSService
    {
        private readonly string _clientId;
        private readonly string _apiKey;
        private readonly string _checksumKey;

        public PayOSService(IOptions<PayOSOption> option)
        {
            var opt = option.Value;
            _clientId = opt.ClientId;
            _apiKey = opt.ApiKey;
            _checksumKey = opt.ChecksumKey;
        }

        public PaymentLinkData CreatePaymentLink(Order order)
        {
            // Ensure order amount is integer
            int amount = (int)Math.Round(order.FinalPrice, 0);
            
            // Generate unique order code (PayOS requires numeric order code)
            long orderCode = long.Parse(DateTimeOffset.UtcNow.ToString("yyMMddHHmmss"));
            
            return new PaymentLinkData
            {
                ClientId = _clientId,
                ApiKey = _apiKey,
                ChecksumKey = _checksumKey,
                OrderCode = orderCode,
                Amount = amount,
                Description = $"DH {order.Order_Id.Substring(0, Math.Min(order.Order_Id.Length, 20))}",
                OrderId = order.Order_Id,
                CancelUrl = "http://localhost:5173/cancel",
                ReturnUrl = "http://localhost:5173/success"
            };
        }
    }

    // Simple data classes to avoid external dependency issues
    public class PaymentLinkData
    {
        public string ClientId { get; set; }
        public string ApiKey { get; set; }
        public string ChecksumKey { get; set; }
        public long OrderCode { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public string OrderId { get; set; }
        public string CancelUrl { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class WebhookData
    {
        public long OrderCode { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
