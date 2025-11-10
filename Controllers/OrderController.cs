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
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllOrders()
        {
            var orders = _orderService.GetAllOrders();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetOrderById(string orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
                return NotFound("Order not found.");
            return Ok(order);
        }

        [HttpGet("user/order/{userId}")]
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult GetOrderByUserId(Guid userId)
        {
            var order = _orderService.GetOrderByUserId(userId);
            if (order == null)
                return NotFound("Order not found for the user.");
            return Ok(order);
        }

        [HttpGet("pending")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPendingOrders()
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

            if (!_orderService.CreateOrder(request))
                return Conflict("Đã có lỗi xảy ra!");

            var createdOrder = _orderService.GetOrderByUserId(request.User_Id);
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
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult Update(string orderId, [FromBody] OrderRequestDTO request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var ok = _orderService.UpdateOrder(orderId, request);
            if (!ok) return NotFound(); 

            return NoContent();
        }
        [HttpDelete("delete/{orderId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string orderId)
        {
            var ok = _orderService.DeleteOrder(orderId);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
