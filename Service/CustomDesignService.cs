using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests.CustomDesign;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Repositories;

namespace HoshiVibe.Service
{
    public class CustomDesignService
    {
        private readonly CustomDesignRepository _repo;
        private readonly CharmRepository _charmRepo;
        private readonly IMapper _mapper;

        public CustomDesignService(CustomDesignRepository repo, CharmRepository charmRepo, IMapper mapper)
        {
            _repo = repo;
            _charmRepo = charmRepo;
            _mapper = mapper;
        }

        public async Task<ICollection<CustomDesignDTO>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _repo.GetAllAsync(ct);
            return list.Select(MapToDTO).ToList();
        }

        public async Task<CustomDesignDTO?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            return entity == null ? null : MapToDTO(entity);
        }

        public async Task<ICollection<CustomDesignDTO>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            var list = await _repo.GetByUserIdAsync(userId, ct);
            return list.Select(MapToDTO).ToList();
        }

        public async Task<CustomDesignDTO> CreateAsync(CustomDesignRqDTO rq, CancellationToken ct = default)
        {
            // Calculate total price from charms
            decimal totalPrice = 0;
            if (rq.CharmIds != null && rq.CharmIds.Any())
            {
                foreach (var charmId in rq.CharmIds)
                {
                    var charm = await _charmRepo.GetByIdAsync(charmId, ct);
                    if (charm != null)
                    {
                        totalPrice += charm.Price;
                    }
                }
            }

            var entity = new CustomDesign
            {
                User_Id = rq.User_Id,
                Name = rq.Name,
                Description = rq.Description,
                Price = totalPrice, // Auto-calculated price
                RawImageBase64 = rq.RawImageBase64,
                AiImageUrl = rq.AiImageUrl
            };

            var created = await _repo.CreateAsync(entity, ct);

            // Add charms
            if (rq.CharmIds != null && rq.CharmIds.Any())
            {
                await _repo.AddCharmsAsync(created.CustomDesign_Id, rq.CharmIds, ct);
            }

            // Reload with charms
            var result = await _repo.GetByIdAsync(created.CustomDesign_Id, ct);
            return MapToDTO(result!);
        }

        public async Task<bool> UpdateAsync(Guid id, CustomDesignRqDTO rq, CancellationToken ct = default)
        {
            var current = await _repo.GetByIdAsync(id, ct);
            if (current == null) return false;

            // Recalculate price if charms changed
            decimal totalPrice = 0;
            if (rq.CharmIds != null && rq.CharmIds.Any())
            {
                foreach (var charmId in rq.CharmIds)
                {
                    var charm = await _charmRepo.GetByIdAsync(charmId, ct);
                    if (charm != null)
                    {
                        totalPrice += charm.Price;
                    }
                }
            }

            current.Name = rq.Name;
            current.Description = rq.Description;
            current.Price = totalPrice;
            current.RawImageBase64 = rq.RawImageBase64;
            current.AiImageUrl = rq.AiImageUrl;
            current.User_Id = rq.User_Id;

            var updated = await _repo.UpdateAsync(current, ct);
            if (!updated) return false;

            // Update charms
            await _repo.RemoveCharmsAsync(id, ct);
            if (rq.CharmIds != null && rq.CharmIds.Any())
            {
                await _repo.AddCharmsAsync(id, rq.CharmIds, ct);
            }

            return true;
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
            => _repo.DeleteAsync(id, ct);

        private CustomDesignDTO MapToDTO(CustomDesign entity)
        {
            return new CustomDesignDTO
            {
                CustomDesign_Id = entity.CustomDesign_Id,
                User_Id = entity.User_Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                RawImageBase64 = entity.RawImageBase64,
                AiImageUrl = entity.AiImageUrl,
                CreatedDate = entity.CreatedDate,
                CharmIds = entity.CustomDesignCharms?.Select(cdc => cdc.CProduct_Id).ToList()
            };
        }
    }
}
