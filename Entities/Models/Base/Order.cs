namespace HoshiVibe.Entities.Models.Base
{
    public class Order
    {
        public required string Order_Id { get; set; }
        public Guid? Cart_Id { get; set; }
        public Guid User_Id { get; set; }
        public  Guid? Voucher_Id { get; set; }
        public required decimal TotalPrice { get; set; }

        public decimal DiscountAmount { get; set; }
        public required decimal FinalPrice  { get; set; }

        public required string ShippingAddress { get; set; }
        public required int PhoneNumber { get; set; }
        public required DateTime OrderDate { get; set; }
        public string? Status { get; set; } = "Pending";

        // Navigation
        public User? User { get; set; }
        public Voucher? Voucher { get; set; }
        public Cart? Cart { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public Payment? Payment { get; set; }
        public CustomProduct? CustomProduct { get; set; }
        }
}
