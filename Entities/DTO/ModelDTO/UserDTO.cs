using HoshiVibe.Entity.Model;

namespace HoshiVibe.Entity.DTO.ModelDTO
{
    public class UserDTO
    {
        public Guid User_Id { get; set; }

        public string Email { get; set; } = string.Empty;
        public required string Account { get; set; }

        public required string Password { get; set; }

        public required string Role { get; set; }

        public bool IsDisabled { get; set; }

        public string? resetToken { get; set; }

        public UserProfileDTO? ProfileDTO { get; set; }
    }
}
