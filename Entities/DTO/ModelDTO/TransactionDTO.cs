namespace HoshiVibe.Entity.DTO.ModelDTO
{
    public class TransactionDTO
    {
        public Guid Transaction_Id { get; set; }
        public Guid Payment_Id { get; set; }
        public int GatewayTransactionId { get; set; }

        public required DateTime TransactionDate { get; set; }
        public required string Status { get; set; } = "Pending";
    }
}
