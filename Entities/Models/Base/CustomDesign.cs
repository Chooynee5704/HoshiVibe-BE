namespace HoshiVibe.Entities.Models.Base
{
    public class CustomDesign
    {
        public Guid CustomDesign_Id { get; set; }
        public Guid? User_Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public string? RawImageBase64 { get; set; } // Original design image
        public string? AiImageUrl { get; set; } // AI-enhanced image
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        // Navigation
        public User? User { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ICollection<CustomDesignCharm>? CustomDesignCharms { get; set; }
    }
}
