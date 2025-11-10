namespace HoshiVibe.Entities.Models.Base
{
    public class Voucher
    {
        public Guid Voucher_Id { get; set; }
        public required string Code { get; set; }
        public required decimal DiscountAmount { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Order>? Orders { get; set; }
    }
}
