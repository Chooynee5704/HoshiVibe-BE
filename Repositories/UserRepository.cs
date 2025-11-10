using HoshiVibe.DB;
using HoshiVibe.Entities.Models.Base;

namespace HoshiVibe.Repositories
{
    public class UserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public bool UserExists(Guid id)
        {
            return _context.Users.Any(u => u.User_Id == id);
        }

        //public ICollection<User> GetAllUsers()
        //{
        //    return _context.Users
        //        .OrderBy(u => u.Role)
        //        .ToList();
        //}
        public User? CheckLogin(string identifier, string password)
        {
            return _context.Users
                .FirstOrDefault(u => ( u.Account == identifier || u.Email == identifier )
                && u.Password == password);
        }
        public User? GetUserById(Guid id)
        {
            return _context.Users
                .FirstOrDefault(u => u.User_Id == id);
        }

        public User? GetUserByEmail(String email)
        {
            return _context.Users
                .FirstOrDefault(u => u.Email == email);
        }
        public User? GetUserByAccount(string account)
        {
            return _context.Users.FirstOrDefault(u => u.Account == account);
        }

        public ICollection<User> GetAllUsers( string role)
        {
            return _context.Users
                .Where(u => u.Role == role)
                .ToList();
        }
        public ICollection<User> GetInactiveUsers()
        {
            return _context.Users
                .Where(u => u.IsDisabled)
                .ToList();
        }


        public bool CreateUser(User user)
        {
            _context.Users.Add(user);
            return Save();
        }
        public bool AccountExists(string account)
        {
            return _context.Users.Any(u => u.Account == account);
        }
        public bool EmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
        public bool UpdateUser(User user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _context.Users.Remove(user);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
