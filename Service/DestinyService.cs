using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests.User;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Repositories;

namespace HoshiVibe.Service
{
    public class DestinyService
    {
        private readonly DestinyRepository _destinyRepository;
        private readonly IMapper _mapper;
        public DestinyService(DestinyRepository destinyRepository, IMapper mapper)
        {
            _destinyRepository = destinyRepository;
            _mapper = mapper;
        }


        public ICollection<Destiny> GetAllDestinies()
        {
            return _destinyRepository.GetAllDestinies();
        }
        public Destiny? GetDestinyById(int id)
        {
            return _destinyRepository.GetDestinyById(id);
        }
        public bool CreateDestiny(Destiny destiny)
        {
            return _destinyRepository.CreateDestiny(destiny);
        }
        public bool UpdateDestiny(int id,DestinyUpdateDTO destiny)
        {
            var existingDestiny = _destinyRepository.GetDestinyById(id);
            if (existingDestiny == null)
            {
                return false; // Destiny not found
            }
            _mapper.Map(destiny, existingDestiny);
            return _destinyRepository.UpdateDestiny(existingDestiny);

        }
        public bool DeleteDestiny(Destiny destiny)
        {
            return _destinyRepository.DeleteDestiny(destiny);
        }
    }
}
