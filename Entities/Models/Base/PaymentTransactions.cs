namespace HoshiVibe.Entities.Models.Base
{
    public class PaymentTransactions
    {
        public Guid Transaction_Id { get; set; }
        public Guid Payment_Id { get; set; }
        public int GatewayTransactionId { get; set; }

        public required DateTime TransactionDate { get; set; }
        public required string Status { get; set; } = "Pending";

        // Navigation
        public Payment? Payment { get; set; }
    }
}
