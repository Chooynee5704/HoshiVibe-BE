namespace HoshiVibe.Entities.Models.Base
{
    public class Destiny
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        public ICollection<UserProfile>? UserProfiles { get; set; }

    }
}
