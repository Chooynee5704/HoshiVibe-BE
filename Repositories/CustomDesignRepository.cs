using HoshiVibe.DB;
using HoshiVibe.Entities.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace HoshiVibe.Repositories
{
    public class CustomDesignRepository
    {
        private readonly DataContext _context;

        public CustomDesignRepository(DataContext context)
        {
            _context = context;
        }

        // === READ ===
        public async Task<ICollection<CustomDesign>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.CustomDesigns
                .Include(cd => cd.CustomDesignCharms)
                    .ThenInclude(cdc => cdc.Charm)
                .OrderByDescending(cd => cd.CreatedDate)
                .ToListAsync(ct);
        }

        public async Task<CustomDesign?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.CustomDesigns
                .Include(cd => cd.CustomDesignCharms)
                    .ThenInclude(cdc => cdc.Charm)
                .FirstOrDefaultAsync(cd => cd.CustomDesign_Id == id, ct);
        }

        public async Task<ICollection<CustomDesign>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.CustomDesigns
                .Include(cd => cd.CustomDesignCharms)
                    .ThenInclude(cdc => cdc.Charm)
                .Where(cd => cd.User_Id == userId)
                .OrderByDescending(cd => cd.CreatedDate)
                .ToListAsync(ct);
        }

        // === CREATE ===
        public async Task<CustomDesign> CreateAsync(CustomDesign entity, CancellationToken ct = default)
        {
            entity.CustomDesign_Id = entity.CustomDesign_Id == Guid.Empty ? Guid.NewGuid() : entity.CustomDesign_Id;
            entity.CreatedDate = DateTime.UtcNow;
            _context.CustomDesigns.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        // === UPDATE ===
        public async Task<bool> UpdateAsync(CustomDesign entity, CancellationToken ct = default)
        {
            var exists = await _context.CustomDesigns.AnyAsync(x => x.CustomDesign_Id == entity.CustomDesign_Id, ct);
            if (!exists) return false;

            _context.CustomDesigns.Update(entity);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        // === DELETE ===
        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var found = await _context.CustomDesigns
                .Include(cd => cd.CustomDesignCharms)
                .FirstOrDefaultAsync(x => x.CustomDesign_Id == id, ct);
            if (found == null) return false;

            _context.CustomDesigns.Remove(found);
            await _context.SaveChangesAsync(ct);
            return true;
        }

        // === CHARM MANAGEMENT ===
        public async Task AddCharmsAsync(Guid customDesignId, List<Guid> charmIds, CancellationToken ct = default)
        {
            var charms = charmIds.Select(charmId => new CustomDesignCharm
            {
                CustomDesignCharm_Id = Guid.NewGuid(),
                CustomDesign_Id = customDesignId,
                CProduct_Id = charmId
            }).ToList();

            _context.CustomDesignCharms.AddRange(charms);
            await _context.SaveChangesAsync(ct);
        }

        public async Task RemoveCharmsAsync(Guid customDesignId, CancellationToken ct = default)
        {
            var charms = await _context.CustomDesignCharms
                .Where(cdc => cdc.CustomDesign_Id == customDesignId)
                .ToListAsync(ct);

            _context.CustomDesignCharms.RemoveRange(charms);
            await _context.SaveChangesAsync(ct);
        }
    }
}
