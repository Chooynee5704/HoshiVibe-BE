using AutoMapper;
using Google.Apis.Auth;
using HoshiVibe.Entities.DTO.ModelRequests.User;
using HoshiVibe.Entity.DTO.ModelRequests.Authen;
using HoshiVibe.Entity.Model;
using HoshiVibe.Repositories;
using HoshiVibe.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HoshiVibe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthenticationController : Controller
    {
        private readonly UserService _userService;
        private readonly JWTService _jwtService;
        public AuthenticationController(UserService userService, JWTService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Identifier) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Vui lòng nhập đầy đủ thông tin.");

            var user = _userService.Login(request.Identifier, request.Password);

            if (user == null)
                return Unauthorized("Tên đăng nhập/email hoặc mật khẩu không đúng.");

            var jwt = _jwtService.GenerateToken(user);
            // Trả về thông tin người dùng, chưa có JWT
            return Ok(new
            {
                Token = jwt,
                user.User_Id,
                user.Email,
                user.Account,
                user.Role,
                Message = "Đăng nhập thành công"
            });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] RegisterDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Account) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Missing required fields.");
            }
            if (!_userService.Register(request, out var user, out var profile, out var cart,"Customer"))
                return Conflict("Account  đã tồn tại. Hoặc email đã được đăng ký ");

            return Ok(new
            {
                user.User_Id,
                user.Email,
                user.Account,
                user.Role,
                Message = "Đăng ký thành công."
            });
        }



        [HttpPost("forgot-password/request")]
        [AllowAnonymous]
        public IActionResult RequestResetPassword([FromBody] ForgotPassRq request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Email is required.");
            if (string.IsNullOrWhiteSpace(request.Account))
                return BadRequest("Account is required.");

            if (!_userService.PasswordReset(request.Account,request.Email, out var code))
                return NotFound("Email not found hoặc gửi email thất bại.");

            return Ok("Verification code sent to your email.");
        }

        [HttpPost("forgot-password/confirm")]
        [AllowAnonymous]
        public IActionResult ConfirmResetPassword([FromBody] ForgotPassConfirm dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Account) ||
                string.IsNullOrWhiteSpace(dto.VerificationCode) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                return BadRequest("Missing required fields.");
            }

            if (!_userService.ConfirmPasswordReset(dto.Email,dto.Account, dto.VerificationCode, dto.NewPassword))
                return BadRequest("Invalid verification code hoặc email không tồn tại.");

            return Ok("Password has been reset successfully.");
        }

    }
}
