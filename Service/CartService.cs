using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests.OderProcess;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Entity.DTO.ModelDTO;
using HoshiVibe.Repository;

namespace HoshiVibe.Service
{
    public class CartService
    {
        private readonly CartRepository _cartRepository;
        private readonly IMapper _mapper;
        public CartService(CartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }
        public CartDTO? GetUserCart(Guid userId)
        {

            var cart = _cartRepository.GetCartByUserId(userId);
            if (cart == null)
                return null;
            return _mapper.Map<CartDTO>(cart);
        }

        public bool CreateCart(CartRequestDTO cartDto)
        {
            var cart = _mapper.Map<Cart>(cartDto);
            cart.Cart_Id = Guid.NewGuid();
            cart.CreatedAt = DateTime.UtcNow;
            cart.UpdatedAt = DateTime.UtcNow;
            return _cartRepository.AddToCart(cart);
        }
        public bool UpdateCart(CartDTO cartDto)
        {
            var existingCart = _cartRepository.GetCartByUserId(cartDto.User_Id);
            if (existingCart == null)
                return false;
            var updatedCart = _mapper.Map(cartDto, existingCart);
            updatedCart.UpdatedAt = DateTime.UtcNow;
            return _cartRepository.UpdateCart(updatedCart);
        }
        public bool DeleteCart(Guid userId)
        {
            var existingCart = _cartRepository.GetCartByUserId(userId);
            if (existingCart == null)
                return false;
            return _cartRepository.DeleteCart(existingCart);
        }
    }
}
