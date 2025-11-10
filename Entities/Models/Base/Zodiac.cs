namespace HoshiVibe.Entities.Models.Base
{
    public class Zodiac
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; // Tính cách, biểu tượng, ý nghĩa

        public string Url { get; set; } = string.Empty;
        public ICollection<UserProfile>? UserProfiles { get; set; }
    }
}
