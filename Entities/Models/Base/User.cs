namespace HoshiVibe.Entities.Models.Base
{
    public class User
    {
        public Guid User_Id { get; set; }

        public string Email { get; set; } = string.Empty;
        public required string Account { get; set; }

        public required string Password { get; set; }

        public required string Role { get; set; }

        public bool IsDisabled { get; set; }

        public string? resetToken { get; set; }

        // Navigation
        public UserProfile? Profile { get; set; }
        public Cart? Cart { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
