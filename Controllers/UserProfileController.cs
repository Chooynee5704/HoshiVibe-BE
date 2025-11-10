using HoshiVibe.Entities.DTO.ModelRequests.User;
using HoshiVibe.Entity.DTO.ModelDTO;
using HoshiVibe.Entity.Model;
using HoshiVibe.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace HoshiVibe.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserAndProfileController : Controller
    {
        private readonly UserProfileService _profileService;
        private readonly UserService _userService;
        public UserAndProfileController(UserProfileService profileService, UserService userService)
        {
            _profileService = profileService;
            _userService = userService;
        }


        [HttpGet("get-user-profile/{userId}")]
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult GetUserProfile(Guid userId)
        {
            var profile = _profileService.GetUserProfileByUserId(userId);
            if (profile == null) return NotFound("User profile not found");
            return Ok(profile);
        }

        [HttpGet("get-all-user-informations")]
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult GetAllUserInformations()
        {
            var users = _profileService.GetAllUserInformations();
            return Ok(users);
        }


        // PUT: api/profile/update-profile/{userId}/{profileId}
        [HttpPut("update-profile/{profileId}")]
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult UpdateProfile(Guid profileId, [FromBody] ProfileUpdateDTO dto)
        {
            var result = _profileService.UpdateProfile(profileId, dto);
            if (!result) return NotFound("User profile not found");
            return Ok("Profile updated successfully");
        }

        // PUT: api/profile/update-user/{id}
        [HttpPut("update-user/{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult UpdateUser(Guid id, [FromBody] UpdateUserDTO dto)
        {
            var result = _profileService.UpdateUser(id, dto.Email, dto.Password);
            if (!result) return NotFound("User not found");
            return Ok("User updated successfully");
        }

        [HttpPost("create-admin-account")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateAdminAccount([FromBody] RegisterDTO request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userService.Register(request, out var user, out var profile, out var cart, "Admin"))
                return Conflict("Account đã tồn tại.");

            return Ok(new
            {
                user.User_Id,
                user.Account,
                user.Role,
                Message = "Đăng ký thành công."
            });
        }

        [HttpDelete("delete-user/{userId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(Guid userId)
        {
            var result = _userService.DeleteUser(userId);
            if (!result) return NotFound("User not found");
            return Ok("User deleted successfully");
        }
    }
}
