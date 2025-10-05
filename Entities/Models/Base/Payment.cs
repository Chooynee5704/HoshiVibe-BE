namespace HoshiVibe.Entities.Models.Base
{
    public class Payment
    {
        public Guid Payment_Id { get; set; }
        public required string Order_Id { get; set; }
        public required string PaymentMethod { get; set; }
        public required decimal Amount { get; set; }
        public required DateTime PaymentDate { get; set; }
        public required string Status { get; set; } = "Pending";

        // Navigation
        public Order? Order { get; set; }
        public ICollection<PaymentTransactions>? Transactions { get; set; }
    }
}
