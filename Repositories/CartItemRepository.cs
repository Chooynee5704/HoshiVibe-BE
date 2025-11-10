using AutoMapper;
using HoshiVibe.DB;
using HoshiVibe.Entities.Models.Base;

namespace HoshiVibe.Repository
{
    public class CartItemRepository
    {
        private readonly DataContext _context;

        public CartItemRepository(DataContext context)
        {
            _context = context;
        }
        public ICollection<CartItem>? GetCartItemsByCartId(Guid cartId)
        {
            return _context.CartItems.Where(ci => ci.Cart_Id == cartId).ToList();
        }
        public CartItem? GetCartItemById(Guid id)
        {
            return  _context.CartItems.FirstOrDefault(ci => ci.CartItem_Id == id);
        }
        public CartItem? GetCartItemByCartIdAndProductId(Guid cartId, Guid productId)
        {
            return _context.CartItems.FirstOrDefault(ci => ci.Cart_Id == cartId && ci.Product_Id == productId);
        }

        public IEnumerable<object> GetMostAddedProducts(int topN)
        {
            var result = _context.CartItems
                .GroupBy(ci => ci.Product_Id)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalAdded = g.Sum(x => x.Quantity),
                    TimesAdded = g.Count()
                })
                .OrderByDescending(x => x.TotalAdded)
                .Take(topN)
                .ToList();

            return result;
        }
        public bool CreateCartItem(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            return Save();
        }
        public bool UpdateCartItem(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            return Save();
        }
        public bool DeleteCartItem(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }


    }
}
