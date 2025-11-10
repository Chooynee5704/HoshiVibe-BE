namespace HoshiVibe.Entities.DTO.ModelRequests.Product
{
    public class CustomPdRqDTO
    {
        public Guid? User_Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public required decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
