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
        public AuthenticationController(UserService userService)
        {
            _userService = userService;
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

            // Trả về thông tin người dùng, chưa có JWT
            return Ok(new
            {
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
            if (request == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userService.Register(request, out var user, out var profile))
                return Conflict("Account đã tồn tại.");

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

            if (!_userService.PasswordReset(request.Email, out var code))
                return NotFound("Email not found hoặc gửi email thất bại.");

            return Ok("Verification code sent to your email.");
        }

        [HttpPost("forgot-password/confirm")]
        [AllowAnonymous]
        public IActionResult ConfirmResetPassword([FromBody] ForgotPassConfirm dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Identifier) ||
                string.IsNullOrWhiteSpace(dto.VerificationCode) ||
                string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                return BadRequest("Missing required fields.");
            }

            if (!_userService.ConfirmPasswordReset(dto.Identifier, dto.VerificationCode, dto.NewPassword))
                return BadRequest("Invalid verification code hoặc email không tồn tại.");

            return Ok("Password has been reset successfully.");
        }

    }
}
