using HoshiVibe.Entities.Models.Base;

namespace HoshiVibe.Entities.DTO.ModelRequests.OderProcess
{
    public class OrderRequestDTO
    {
        public Guid User_Id { get; set; }

        public Guid? Voucher_Id { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal DiscountAmount { get; set; }
        public decimal FinalPrice { get; set; }
        
        // Optional for pending orders, required only at checkout
        public string? ShippingAddress { get; set; }
        public int? PhoneNumber { get; set; }
        
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string? Status { get; set; } = "Pending";
        public string? ShippingStatus { get; set; } = "Pending";
    }
}
