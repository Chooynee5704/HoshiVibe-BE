using HoshiVibe.DB;
using HoshiVibe.Entities.Models.Base;
using Microsoft.EntityFrameworkCore; // <-- Add this using directive

namespace HoshiVibe.Repository
{
    public class CartRepository
    {   
        private readonly DataContext _context;
        
        public CartRepository(DataContext context)
        {
            _context = context;
        }

        public Cart? GetCartByUserId(Guid userId)
        {
            return _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.User_Id == userId);
        }

        public bool AddToCart(Cart cart)
        {
            _context.Carts.Add(cart);
            return Save();
        }
        public bool UpdateCart(Cart cart)
        {
            _context.Carts.Update(cart);
            return Save();
        }
        public bool DeleteCart(Cart cart)
        {
            _context.Carts.Remove(cart);
            return Save();
        }
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
