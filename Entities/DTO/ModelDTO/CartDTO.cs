namespace HoshiVibe.Entity.DTO.ModelDTO
{
    public class CartDTO
    {
        public Guid Cart_Id { get; set; }
        public Guid User_Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<CartItemDTO>? CartItems { get; set; }
    }
}
