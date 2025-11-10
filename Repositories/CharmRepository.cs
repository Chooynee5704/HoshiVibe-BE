using HoshiVibe.DB;
using HoshiVibe.Entities.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace HoshiVibe.Repositories
{
    public class CharmRepository
    {
        private readonly DataContext _context;

        public CharmRepository(DataContext context)
        {
            _context = context;
        }

        // === READ ===
        public async Task<ICollection<CustomProduct>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.CustomProducts
                .OrderBy(p => p.Price)
                .ToListAsync(ct);
        }

        public async Task<CustomProduct?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.CustomProducts
                .FirstOrDefaultAsync(u => u.CProduct_Id == id, ct);
        }

        public async Task<ICollection<CustomProduct>> SearchAsync(string? keyword, CancellationToken ct = default)
        {
            var q = _context.CustomProducts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var k = keyword.ToLower();
                q = q.Where(p =>
                    (p.Name != null && p.Name.ToLower().Contains(k)) ||
                    (p.Category != null && p.Category.ToLower().Contains(k)));
            }

            return await q.OrderBy(p => p.Price).ToListAsync(ct);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.CustomProducts.AnyAsync(u => u.CProduct_Id == id, ct);
        }

        // === CREATE ===
        public async Task<CustomProduct> CreateAsync(CustomProduct entity, CancellationToken ct = default)
        {
            entity.CProduct_Id = entity.CProduct_Id == Guid.Empty ? Guid.NewGuid() : entity.CProduct_Id;
            _context.CustomProducts.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        // === UPDATE ===
        public async Task<bool> UpdateAsync(CustomProduct entity, CancellationToken ct = default)
        {
            var exists = await _context.CustomProducts.AnyAsync(x => x.CProduct_Id == entity.CProduct_Id, ct);
            if (!exists) return false;

            _context.CustomProducts.Update(entity);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        // === DELETE ===
        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var found = await _context.CustomProducts.FirstOrDefaultAsync(x => x.CProduct_Id == id, ct);
            if (found == null) return false;

            _context.CustomProducts.Remove(found);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
