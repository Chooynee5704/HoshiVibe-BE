using HoshiVibe.Entities.Models.Base;

namespace HoshiVibe.Entities.DTO.ModelRequests.OderProcess
{
    public class OrderRequestDTO
    {
        public Guid User_Id { get; set; }

        public Guid? Voucher_Id { get; set; }

        public required decimal TotalPrice { get; set; }

        public required decimal DiscountAmount { get; set; }
        public required decimal FinalPrice { get; set; }
        public required string ShippingAddress { get; set; }
        public required int PhoneNumber { get; set; }
        public required DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string? Status { get; set; } = "Pending";
    }
}
