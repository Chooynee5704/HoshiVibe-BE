namespace HoshiVibe.Entity.DTO.ModelDTO
{
    public class PaymentDTO
    {
        public Guid Payment_Id { get; set; }
        public string Order_Id { get; set; }
        public required string PaymentMethod { get; set; }
        public required decimal Amount { get; set; }
        public required DateTime PaymentDate { get; set; }
        public required string Status { get; set; } = "Pending";
    }
}
