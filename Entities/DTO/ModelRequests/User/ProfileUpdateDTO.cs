namespace HoshiVibe.Entities.DTO.ModelRequests.User
{
    public class ProfileUpdateDTO
    {
        public string? AvatarUrl { get; set; } 

        public string? FullName { get; set; }
        public int Point { get; set; } = 0;

        public int Age { get; set; }

        public DateTime Yob { get; set; }
        public string? YobDestination { get; set; }

        //public int? ZodiacId { get; set; }
        //public int? DestinyId { get; set; }
    }
}
