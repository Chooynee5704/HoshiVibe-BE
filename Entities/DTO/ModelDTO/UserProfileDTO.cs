using HoshiVibe.Entities.Models.Base;

namespace HoshiVibe.Entity.Model
{
    public class UserProfileDTO
    {
        public required Guid User_Id { get; set; }
        public Guid UserProfile_Id { get; set; }

        public string? AvatarUrl { get; set; }

        public string FullName { get; set; } = string.Empty;
        public int Point { get; set; } = 0;

        public  int Age { get; set; } 
        public string Address { get; set; } = string.Empty;

        public DateTime Yob { get; set; }
        public string YobDestination { get; set; } = string.Empty;
        public string Zodiac { get; set; } = string.Empty;

        public string? ZodiacUrl { get; set; }
    }
}
