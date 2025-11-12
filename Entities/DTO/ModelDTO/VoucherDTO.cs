namespace HoshiVibe.Entity.DTO.ModelDTO
{
    public class VoucherDTO
    {
        public Guid Voucher_Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string VoucherName { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public int UseTime { get; set; }
        public int UsedCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation
        public ICollection<OrderDTO>? Orders { get; set; }
    }
}
