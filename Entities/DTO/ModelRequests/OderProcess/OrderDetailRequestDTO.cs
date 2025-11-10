namespace HoshiVibe.Entities.DTO.ModelRequests.OderProcess
{
    public class OrderDetailRequestDTO
    {
 
        public required string OrderId { get; set; }

        public Guid? ProductId { get; set; }
        public Guid? CProduct_Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity * (1 - Discount);
    }
}
