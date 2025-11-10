using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests.Product;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Repositories;

namespace HoshiVibe.Service
{
    public class CharmService
    {
        private readonly CharmRepository _repo;
        private readonly IMapper _mapper;

        public CharmService(CharmRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ICollection<CustomProductDTO>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _repo.GetAllAsync(ct);
            return _mapper.Map<ICollection<CustomProductDTO>>(list);
        }

        public async Task<CustomProductDTO?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct);
            return _mapper.Map<CustomProductDTO?>(entity);
        }

        public async Task<ICollection<CustomProductDTO>> SearchAsync(string? keyword, CancellationToken ct = default)
        {
            var list = await _repo.SearchAsync(keyword, ct);
            return _mapper.Map<ICollection<CustomProductDTO>>(list);
        }

        public async Task<CustomProductDTO> CreateAsync(CustomPdRqDTO rq, CancellationToken ct = default)
        {
            var entity = _mapper.Map<CustomProduct>(rq);
            var created = await _repo.CreateAsync(entity, ct);
            return _mapper.Map<CustomProductDTO>(created);
        }

        public async Task<bool> UpdateAsync(Guid id, CustomPdRqDTO rq, CancellationToken ct = default)
        {
            var current = await _repo.GetByIdAsync(id, ct);
            if (current == null) return false;

            // map các field từ rq -> current, bỏ qua null
            _mapper.Map(rq, current);
            current.CProduct_Id = id;

            return await _repo.UpdateAsync(current, ct);
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
            => _repo.DeleteAsync(id, ct);
    }
}
