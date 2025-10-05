namespace HoshiVibe.Entities.DTO.ModelRequests.Product
{
    public class ProductRequestDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required string Category { get; set; }
        public required int Stock { get; set; }
        public required string Status { get; set; }
        public string? ImageUrl { get; set; }
    }
}
