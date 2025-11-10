namespace HoshiVibe.Entity.DTO.ModelDTO
{
    public class CartItemDTO
    {
        public Guid CartItem_Id { get; set; }
        public Guid Cart_Id { get; set; }
        public Guid Product_Id { get; set; }

        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }    // giá tại thời điểm thêm
        public decimal TotalPrice => UnitPrice * Quantity;


    }
}
