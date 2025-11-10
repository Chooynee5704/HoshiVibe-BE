using HoshiVibe.DB;

namespace HoshiVibe.Repositories
{
    public class VoucherRepository
    {
        private readonly DataContext _context;
        public VoucherRepository(DataContext context)
        {
            _context = context;
        }
        public bool VoucherExists(Guid id)
        {
            return _context.Vouchers.Any(v => v.Voucher_Id == id);
        }
    }
}
