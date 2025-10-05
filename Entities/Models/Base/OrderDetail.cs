namespace HoshiVibe.Entities.Models.Base
{
    public class OrderDetail
    {
        public Guid OrderDetail_Id { get; set; }
        public required string OrderId { get; set; }
        
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity * (1 - Discount);

        // Navigation
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
