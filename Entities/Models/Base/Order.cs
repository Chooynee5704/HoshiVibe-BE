namespace HoshiVibe.Entities.Models.Base
{
    public class Order
    {
        public required string Order_Id { get; set; }
        public Guid? Cart_Id { get; set; }
        public Guid User_Id { get; set; }
        public  Guid? Voucher_Id { get; set; }
        public decimal TotalPrice { get; set; }

        public decimal DiscountAmount { get; set; }
        public decimal FinalPrice  { get; set; }

        // Optional for pending orders, required only at checkout
        public string? ShippingAddress { get; set; }
        public int? PhoneNumber { get; set; }
        
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; } = "Pending";
        public string? ShippingStatus { get; set; } = "Pending"; // Pending, Shipping, Delivered, PickedUp

        // Navigation
        public User? User { get; set; }
        public Voucher? Voucher { get; set; }
        public Cart? Cart { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public Payment? Payment { get; set; }
        public CustomProduct? CustomProduct { get; set; }
        }
}
