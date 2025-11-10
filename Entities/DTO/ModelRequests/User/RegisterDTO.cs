using System.ComponentModel.DataAnnotations;

namespace HoshiVibe.Entities.DTO.ModelRequests.User
{
    public class RegisterDTO
    {

        [Required(ErrorMessage = "Account is required")]
        [StringLength(50, ErrorMessage = "Account cannot exceed 50 characters")]
        public string Account { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        public required string  Password { get; set; } 

        public string Role { get; set; } = "Customer";
        public bool IsDisabled { get; set; } = false;
    }
}
