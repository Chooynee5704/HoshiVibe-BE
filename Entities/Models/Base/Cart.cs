namespace HoshiVibe.Entities.Models.Base
{
    public class Cart
    {
        public Guid Cart_Id { get; set; }
        public Guid User_Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User? User { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }
    }
}
