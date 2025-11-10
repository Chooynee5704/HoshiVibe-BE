using HoshiVibe.DB;
using HoshiVibe.Entities.Models.Base;

namespace HoshiVibe.Repositories
{
    public class ZodiacRepository
    {
        private readonly DataContext _context;
        public ZodiacRepository(DataContext context)
        {
            _context = context;
        }
        public ICollection<Zodiac> GetAllZodiacSigns() {
                return _context.Zodiacs.ToList();
        }
        public Zodiac? GetZodiacById(int id) {
                return _context.Zodiacs
                    .FirstOrDefault(z => z.Id == id);
        }
        public bool CreateZodiac(Zodiac zodiac)
        {
            _context.Zodiacs.Add(zodiac);
            return Save();
        }
        public bool UpdateZodiac(Zodiac zodiac)
        {
            _context.Zodiacs.Update(zodiac);
            return Save();
        }
        public bool DeleteZodiac(Zodiac zodiac)
        {
            _context.Zodiacs.Remove(zodiac);
            return Save();
        }
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

    }
}
