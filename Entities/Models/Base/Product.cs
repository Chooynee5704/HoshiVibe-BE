namespace HoshiVibe.Entities.Models.Base
{
    public class Product
    {
        public Guid Product_Id { get; set; }

        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required string Category { get; set; }
        public string? Destiny { get; set; }
        public required int Stock { get; set; }
        public string? ImageUrl { get; set; }
        public string? Status { get; set; }

        // Navigation
        public ICollection<OrderDetail>? OrderDetails { get; set; }

    }
}
