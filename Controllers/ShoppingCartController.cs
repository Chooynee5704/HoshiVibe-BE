using HoshiVibe.Entities.DTO.ModelRequests.OderProcess;
using HoshiVibe.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HoshiVibe.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ShoppingCartController : Controller
    {
        private readonly CartService _cartService;
        private readonly CartItemsService _cartItemsService;
        public ShoppingCartController(CartService cartService, CartItemsService cartItemsService)
        {
            _cartService = cartService;
            _cartItemsService = cartItemsService;
        }

        [HttpGet("getUserCart{userId}")]
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult GetUserCart(Guid userId)
        {
            var cart = _cartService.GetUserCart(userId);
            if (cart == null)
                return NotFound("Cart not found for the user.");
            return Ok(cart);
        }

        [HttpPost("create-shopping-cart")]
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult Create([FromBody] CartRequestDTO cartDto)
        {
            if (cartDto == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_cartService.CreateCart(cartDto))
                return Conflict("Đã có lỗi xảy ra!");

            return Ok("Tạo giỏ hàng thành công");
        }

        [HttpPost("add-to-cart")]
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult AddToCart([FromBody] CartItemsRequestDTO requestDTO)
        {
            if (requestDTO == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_cartItemsService.CreateCartItem(requestDTO))
                return Conflict("Đã có lỗi xảy ra!");

            return Ok("Thêm vào giỏ hàng thành công");
        }


        [HttpPut("update-shopping-cart-item{cartItemId}")]
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult Update(Guid cartItemId, [FromBody] CartItemsRequestDTO requestDTO)
        {
            if (requestDTO == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_cartItemsService.UpdateCartItem(cartItemId, requestDTO))
                return Conflict("Đã có lỗi xảy ra!");
            return Ok("Cập nhật giỏ hàng thành công");
        }

        [HttpDelete("delete-shopping-cart-item{cartItemId}")]
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult Delete(Guid cartItemId)
        {
            if (!_cartItemsService.DeleteCartItem(cartItemId))
                return Conflict("Đã có lỗi xảy ra!");
            return Ok("Xóa sản phẩm khỏi giỏ hàng thành công");
        }
    }
}
