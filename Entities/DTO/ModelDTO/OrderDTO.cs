using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Entity.Model;

namespace HoshiVibe.Entity.DTO.ModelDTO
{
    public class OrderDTO
    {
        public required string Order_Id { get; set; }
        public  Guid User_Id { get; set; }

        public  Guid? Voucher_Id { get; set; }
        public required decimal TotalPrice { get; set; }

        public required decimal DiscountAmount { get; set; }
        public required decimal FinalPrice  { get; set; }
        public required string ShippingAddress { get; set; }

        public required int PhoneNumber { get; set; }
        public required DateTime OrderDate { get; set; }
        public string? Status { get; set; } = "Pending";

        public ICollection<OrderDetailDTO>? OrderDetails { get; set; }

    }
}
