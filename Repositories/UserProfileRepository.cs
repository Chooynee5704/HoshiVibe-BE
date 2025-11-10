using HoshiVibe.DB;
using HoshiVibe.Entities.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace HoshiVibe.Repositories
{
    public class UserProfileRepository
    {
        private readonly DataContext _context;
        public UserProfileRepository(DataContext dataContext) { _context = dataContext; }

        public bool UserExists(Guid id)
            => _context.UserProfiles.Any(u => u.User_Id == id);

        // ===== READ: Danh sách user + profile + zodiac/destiny (no-tracking) =====
        public ICollection<User> GetAllUserInformations()
        {
            return _context.Users
                .Include(u => u.Profile).ThenInclude(p => p.Zodiac)
                .Include(u => u.Profile).ThenInclude(p => p.Destiny)
                .AsNoTracking()
                .ToList();
        }

        // ===== READ for UPDATE: lấy UserProfile theo ProfileId (cần tracking) =====
        public UserProfile? GetUserProfileByProfileId(Guid profileId)
        {
            return _context.UserProfiles
                .Include(p => p.Zodiac)
                .Include(p => p.Destiny)
                .FirstOrDefault(p => p.UserProfile_Id == profileId);
        }

        // ===== READ: lấy UserProfile theo UserId (no-tracking) =====
        public UserProfile? GetUserProfileByUserId(Guid userId)
        {
            return _context.UserProfiles
                .Include(p => p.Zodiac)   // ✅ ĐÚNG: Include trên UserProfile
                .Include(p => p.Destiny)
                .AsNoTracking()
                .FirstOrDefault(p => p.User_Id == userId);
        }

        // ===== READ: lấy User (kèm Profile) theo UserId (no-tracking) =====
        public User? GetProfileByUserId(Guid id)
        {
            return _context.Users
                .Include(u => u.Profile).ThenInclude(p => p.Zodiac)
                .Include(u => u.Profile).ThenInclude(p => p.Destiny)
                .AsNoTracking()
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

        private bool Save() => _context.SaveChanges() > 0;
    }
}
