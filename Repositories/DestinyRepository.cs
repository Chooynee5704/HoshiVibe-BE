using HoshiVibe.DB;
using HoshiVibe.Entities.Models.Base;

namespace HoshiVibe.Repositories
{
    public class DestinyRepository
    {
        private readonly DataContext _context;
        public DestinyRepository(DataContext context)
        {
            _context = context;
        }
        public ICollection<Destiny> GetAllDestinies() {
                return _context.Destinies.ToList();
        }
        public Destiny? GetDestinyById(int id) {
                return _context.Destinies
                    .FirstOrDefault(d => d.Id == id);
        }
        public bool CreateDestiny(Destiny destiny)
        {
            _context.Destinies.Add(destiny);
            return Save();
        }
        public bool UpdateDestiny(Destiny destiny)
        {
            _context.Destinies.Update(destiny);
            return Save();
        }
        public bool DeleteDestiny(Destiny destiny)
        {
            _context.Destinies.Remove(destiny);
            return Save();
        }
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
