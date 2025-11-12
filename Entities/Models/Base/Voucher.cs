namespace HoshiVibe.Entities.Models.Base
{
    public class Voucher
    {
        public Guid Voucher_Id { get; set; }
        public required string Code { get; set; }
        public required string VoucherName { get; set; }
        public required decimal DiscountAmount { get; set; }
        public int UseTime { get; set; } = 10; // Number of times voucher can be used
        public int UsedCount { get; set; } = 0; // Number of times voucher has been used
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Order>? Orders { get; set; }
    }
}
