using HoshiVibe.DB;
using HoshiVibe.Entities.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace HoshiVibe.Repositories
{
    public class UserProfileRepository
    {
        private readonly DataContext _context;
        public UserProfileRepository(DataContext dataContext ) { _context = dataContext; }

        public bool UserExists(Guid id)
        {
            return _context.UserProfiles.Any(u => u.User_Id == id);
        }

        public ICollection<User> GetAllUserInformations()
        {
            return _context.Users.Include(u => u.Profile).ToList();
        }

        public UserProfile? GetUserProfile(Guid id)
        {
            return _context.UserProfiles
                .FirstOrDefault(u => u.User_Id == id);
        }
        public User? GetProfileByUserId(Guid id)
        {
            return _context.Users
                .Include(u => u.Profile)
                .FirstOrDefault(u => u.User_Id == id);
        }
        public bool CreateUserProfile(UserProfile user)
        {
            _context.UserProfiles.Add(user);
            return Save();
        }
        public bool UpdateProfile(UserProfile user)
        {
            _context.UserProfiles.Update(user);
            return Save();
        }

        public bool DeleteUser(UserProfile user)
        {
            _context.UserProfiles.Remove(user);
            return Save();
        }
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
