using AutoMapper;
using Azure.Core;
using HoshiVibe.Entities.DTO.ModelRequests.OderProcess;
using HoshiVibe.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HoshiVibe.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("all")]
        public IActionResult GetAllOrders()
        {
            var orders = _orderService.GetAllOrders();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrderById(string orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
                return NotFound("Order not found.");
            return Ok(order);
        }

        [HttpGet("user/order/{userId}")]
        public IActionResult GetOrderByUserId(Guid userId)
        {
            var order = _orderService.GetOrderByUserId(userId);
            if (order == null)
                return NotFound("Order not found for the user.");
            return Ok(order);
        }

        [HttpGet("user/orders/{userId}")]
        public IActionResult GetAllOrdersByUserId(Guid userId)
        {
            var orders = _orderService.GetAllOrdersByUserId(userId);
            return Ok(orders);
        }

        [HttpGet("pending")]
        public IActionResult GetPendingOrder()
        {
            // Get the current user's ID from the JWT token
            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Unauthorized("Invalid user token.");
            }

            var order = _orderService.GetUserPendingOrder(userId);
            if (order == null)
                return NotFound("No pending order found for the user.");
            
            return Ok(order);
        }

        [HttpGet("pending/all")]
        public IActionResult GetAllPendingOrders()
        {
            var orders = _orderService.GetPendingOrders();
            return Ok(orders);
        }
        [HttpPost("create")]
        [AllowAnonymous]
        public IActionResult Create([FromBody] OrderRequestDTO request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var createdOrder = _orderService.CreateOrder(request);
            if (createdOrder == null)
                return Conflict("Đã có lỗi xảy ra!");

            return Ok(new
            {
                createdOrder.User_Id,
                createdOrder.Order_Id,
                createdOrder.FinalPrice,
                createdOrder.Status,
                createdOrder.OrderDetails,
                Message = "Tạo mới thành công."
            });
        }
        [HttpPut("update/{orderId}")]
        public IActionResult Update(string orderId, [FromBody] OrderRequestDTO request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = _orderService.UpdateOrder(orderId, request);
            if (!ok) return NotFound(); 

            return NoContent();
        }

        [HttpPut("update-shipping-status/{orderId}")]
        public IActionResult UpdateShippingStatus(string orderId, [FromBody] UpdateShippingStatusDTO request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = _orderService.UpdateShippingStatus(orderId, request.ShippingStatus);
            if (!ok) return NotFound("Order not found or invalid shipping status.");

            return Ok(new { message = "Shipping status updated successfully." });
        }

        [HttpDelete("delete/{orderId}")]
        public IActionResult Delete(string orderId)
        {
            var ok = _orderService.DeleteOrder(orderId);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
