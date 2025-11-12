namespace HoshiVibe.Entities.DTO.ModelRequests.CustomDesign
{
    public class CustomDesignDTO
    {
        public Guid CustomDesign_Id { get; set; }
        public Guid? User_Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public string? RawImageBase64 { get; set; }
        public string? AiImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Guid>? CharmIds { get; set; } // List of charm IDs
    }
}
