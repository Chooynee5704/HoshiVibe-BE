namespace HoshiVibe.Entities.DTO.ModelRequests.CustomDesign
{
    public class CustomDesignRqDTO
    {
        public Guid? User_Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; } // Optional - auto-calculated from charms
        public string? RawImageBase64 { get; set; }
        public string? AiImageUrl { get; set; }
        public List<Guid>? CharmIds { get; set; } // List of charm IDs to include
    }
}
