using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests;
using HoshiVibe.Entities.DTO.ModelRequests.User;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Mail;

namespace HoshiVibe.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepo;
        private readonly UserProfileRepository _userProfileRepo;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly string _fromEmail = "hoshivibe8386@gmail.com";
        private readonly string _appPassword = "ewtxsjvfazadkeuv";

        public UserService(UserRepository userRepo, UserProfileRepository userProfileRepo, IMapper mapper, PasswordHasher<User> passwordHasher)
        {
            _userRepo = userRepo;
            _userProfileRepo = userProfileRepo;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public User? Login(string identifier, string password)
        {
            var user = _userRepo.GetUserByAccount(identifier) ?? _userRepo.GetUserByEmail(identifier);

            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            return user;
        }
        public bool Register(RegisterDTO dto, out User? user, out UserProfile? userProfile)
        {
            user = null;
            userProfile = null;

            if (_userRepo.AccountExists(dto.Account))
                return false;


            user = _mapper.Map<User>(dto);
            user.User_Id = Guid.NewGuid();
            user.Role = "Customer";
            user.IsDisabled = false;


            user.Password = _passwordHasher.HashPassword(user, dto.Password);

    
            userProfile = new UserProfile
            {
                UserProfile_Id = Guid.NewGuid(),
                User_Id = user.User_Id,
                AvatarUrl = string.Empty,
                FullName = string.Empty,
                Point = 0,
                Age = 0,
                Address = string.Empty,
                Yob = DateTime.MinValue,  // default
                YobDestination = string.Empty,
                Zodiac = string.Empty,
                ZodiacUrl = string.Empty
            };

            return _userRepo.CreateUser(user) && _userProfileRepo.CreateUserProfile(userProfile);
        }


        public bool PasswordReset(string email, out string? verificationCode)
        {
            verificationCode = null;

            var user = _userRepo.GetUserByEmail(email);
            if (user == null) return false;

            // Create Verifcation Code
            verificationCode = new Random().Next(100000, 999999).ToString();
            user.resetToken = verificationCode;

            if (!_userRepo.UpdateUser(user)) return false;

            return SendVerificationCode(email, verificationCode);
        }


        public bool ConfirmPasswordReset(string identifier, string verificationCode, string newPassword)
        {
            var user = _userRepo.GetUserByAccount(identifier) ?? _userRepo.GetUserByEmail(identifier);
            if (user == null) return false;

            if (user.resetToken != verificationCode)
                return false;

            // Hash mật khẩu mới
            user.Password = _passwordHasher.HashPassword(user, newPassword);
            user.resetToken = null;

            return _userRepo.UpdateUser(user);
        }

        public bool SendVerificationCode(string toEmail, string code)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(_fromEmail);
                mail.To.Add(toEmail);
                mail.Subject = "Mã xác thực đặt lại mật khẩu";
                mail.Body = $"Xin chào,\n\nMã xác thực của bạn là: {code}\n\nVui lòng không chia sẻ mã này với bất kỳ ai.";

                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(_fromEmail, _appPassword),
                    EnableSsl = true
                };

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Gửi email thất bại: " + ex.Message);
                return false;
            }
        }
    }
}
