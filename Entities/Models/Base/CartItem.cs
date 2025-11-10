namespace HoshiVibe.Entities.Models.Base
{
    public class CartItem
    {
        public Guid CartItem_Id { get; set; }
        public Guid Cart_Id { get; set; }
        public Guid Product_Id { get; set; }

        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }    // giá tại thời điểm thêm
        public decimal TotalPrice => UnitPrice * Quantity;

        // Navigation
        public Cart? Cart { get; set; }
        public Product? Product { get; set; }
    }
}
