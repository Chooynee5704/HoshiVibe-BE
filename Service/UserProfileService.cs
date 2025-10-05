using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests.User;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Entity.DTO.ModelDTO;
using HoshiVibe.Repositories;
using Microsoft.AspNetCore.Identity;

namespace HoshiVibe.Service
{
    public class UserProfileService
    {
        private readonly UserRepository _userRepo;
        private readonly UserProfileRepository _userProfileRepo;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserProfileService(
            UserRepository userRepo,
            UserProfileRepository userProfileRepo,
            IMapper mapper,
            PasswordHasher<User> passwordHasher)
        {
            _userRepo = userRepo;
            _userProfileRepo = userProfileRepo;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }
        public ICollection<UserDTO> GetAllUserInformations()
        {
            var users = _userProfileRepo.GetAllUserInformations();
            if (users == null || users.Count == 0)
            {
                throw new InvalidOperationException("No users found");
            }
            return _mapper.Map<ICollection<UserDTO>>(users);
        }

        public UserDTO GetUserProfileByUserId(Guid id )
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("UserId không hợp lệ", nameof(id));
            }

            var existingUserProfile = _userProfileRepo.GetProfileByUserId(id);
            if (existingUserProfile == null) {
                throw new KeyNotFoundException("User profile not found");
            }

            return _mapper.Map<UserDTO>(existingUserProfile);
        }
        public bool UpdateProfile(ProfileUpdateDTO dto)
        {
            var existingProfile = _userProfileRepo.GetUserProfile(dto.User_Id);
            if (existingProfile == null) return false;

            _mapper.Map(dto, existingProfile); // map DTO -> entity
            return _userProfileRepo.UpdateProfile(existingProfile);
        }


        public bool UpdateUser(Guid userId, string email, string password)
        {
            var user = _userRepo.GetUserById(userId);
            if (user == null) return false;

            if (!string.IsNullOrWhiteSpace(email))
            {
                user.Email = email;
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                user.Password = _passwordHasher.HashPassword(user, password);
            }

            return _userRepo.UpdateUser(user);
        }


    }
}
