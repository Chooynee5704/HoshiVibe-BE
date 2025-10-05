namespace HoshiVibe.Entities.DTO.ModelRequests.User
{
    public class ProfileUpdateDTO
    {
        public Guid UserProfile_Id { get; set; }
        public required Guid User_Id { get; set; }
        public string? AvatarUrl { get; set; } 

        public string? FullName { get; set; }
        public int Point { get; set; } = 0;

        public int Age { get; set; }
        public string? Address { get; set; } 

        public DateTime Yob { get; set; }
        public string? YobDestination { get; set; }
        public string? Zodiac { get; set; }

        public string? ZodiacUrl { get; set; } = string.Empty;

    }
}
