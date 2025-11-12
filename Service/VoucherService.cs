using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Repositories;

namespace HoshiVibe.Service
{
    public class VoucherService
    {
        private readonly VoucherRepository _voucherRepository;

        public VoucherService(VoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<Voucher?> ValidateVoucherCode(string code)
        {
            var voucher = await _voucherRepository.GetVoucherByCode(code);
            
            if (voucher == null)
            {
                return null; // Voucher not found
            }

            // Check if voucher is active
            if (!voucher.IsActive)
            {
                return null;
            }

            // Check if voucher is within valid date range
            var now = DateTime.UtcNow;
            if (now < voucher.StartDate || now > voucher.EndDate)
            {
                return null;
            }

            // Check if voucher has remaining uses
            if (voucher.UsedCount >= voucher.UseTime)
            {
                return null;
            }

            return voucher;
        }

        public async Task<List<Voucher>> GetAllVouchers()
        {
            return await _voucherRepository.GetAllVouchers();
        }

        public async Task<Voucher> CreateVoucher(Voucher voucher)
        {
            return await _voucherRepository.CreateVoucher(voucher);
        }

        public async Task<bool> UseVoucher(Guid voucherId)
        {
            return await _voucherRepository.IncrementUsedCount(voucherId);
        }
    }
}
