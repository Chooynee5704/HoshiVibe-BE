using HoshiVibe.DB;
using HoshiVibe.Entities.Models.Base;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Voucher?> GetVoucherByCode(string code)
        {
            return await _context.Vouchers
                .FirstOrDefaultAsync(v => v.Code == code);
        }

        public async Task<List<Voucher>> GetAllVouchers()
        {
            return await _context.Vouchers.ToListAsync();
        }

        public async Task<Voucher> CreateVoucher(Voucher voucher)
        {
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }

        public async Task<bool> UpdateVoucher(Voucher voucher)
        {
            _context.Vouchers.Update(voucher);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IncrementUsedCount(Guid voucherId)
        {
            var voucher = await _context.Vouchers.FindAsync(voucherId);
            if (voucher == null) return false;
            
            voucher.UsedCount++;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
