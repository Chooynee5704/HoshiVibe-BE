namespace HoshiVibe.Entities.Models.Base
{
    public class CustomProduct
    {
        public Guid CProduct_Id { get; set; }
        public  string? Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        // Navigation
        public OrderDetail? OrderDetails { get; set; }
    }
}
