namespace HoshiVibe.Entity.DTO.ModelDTO
{
    public class VoucherDTO
    {
        public Guid Voucher_Id { get; set; }
        public required string Code { get; set; }
        public required decimal DiscountAmount { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<OrderDTO>? Orders { get; set; }
    }
}
