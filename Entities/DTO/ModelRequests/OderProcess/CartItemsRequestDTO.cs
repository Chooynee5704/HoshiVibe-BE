namespace HoshiVibe.Entities.DTO.ModelRequests.OderProcess
{
    public class CartItemsRequestDTO
    {
        public Guid Cart_Id { get; set; }

        public Guid Product_Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity ;
    }
}
