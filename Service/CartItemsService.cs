using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests.OderProcess;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Entity.DTO.ModelDTO;
using HoshiVibe.Repositories;
using HoshiVibe.Repository;

namespace HoshiVibe.Service
{
    public class CartItemsService
    {
        private readonly CartItemRepository _cartItemRepository;
        private readonly IMapper _mapper;

        public CartItemsService(CartItemRepository cartItemRepository, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
        }
        public ICollection<CartItemDTO>? GetCartItemsByCartId(Guid cartId)
        {
            var cartItems = _cartItemRepository.GetCartItemsByCartId(cartId);
            if (cartItems == null)
                return null;
            return _mapper.Map<ICollection<CartItemDTO>>(cartItems);
        }
        public bool CreateCartItem(CartItemsRequestDTO request)
        {
            var cartItem = _cartItemRepository.GetCartItemByCartIdAndProductId(request.Cart_Id, request.Product_Id);
            if (cartItem == null)
            {
                cartItem = _mapper.Map<CartItem>(request);
                cartItem.CartItem_Id = Guid.NewGuid();
                return _cartItemRepository.CreateCartItem(cartItem);
            }
            else
            {
                cartItem.Quantity += request.Quantity;
                cartItem.UnitPrice = request.UnitPrice;
                return _cartItemRepository.UpdateCartItem(cartItem);

            }
        }
        public bool UpdateCartItem(Guid id, CartItemsRequestDTO request)
        {
            var existingCartItem = _cartItemRepository.GetCartItemById(id);
            if (existingCartItem == null) return false;
            var updatedCartItem = _mapper.Map(request, existingCartItem);
            return _cartItemRepository.UpdateCartItem(updatedCartItem);
        }
        public bool DeleteCartItem(Guid id)
        {
            var existingCartItem = _cartItemRepository.GetCartItemById(id);
            if (existingCartItem == null) return false;
            return _cartItemRepository.DeleteCartItem(existingCartItem);
        }
    }
}
