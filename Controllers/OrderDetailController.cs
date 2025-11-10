using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests.OderProcess;
using HoshiVibe.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HoshiVibe.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class OrderDetailController : Controller
    {
        private readonly OrderDetaillsService _service;

        public OrderDetailController (OrderDetaillsService service)
        {
            _service = service;
        }

        [HttpGet("order/{orderId}")]
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult GetOrderDetailsByOrderId(string orderId)
        {
            var orderDetails = _service.GetOrderDetailsByOrderId(orderId);

            if (orderDetails == null || !orderDetails.Any())
                return NotFound("No order details found for the specified order ID.");

            return Ok(orderDetails);
        }
        [HttpPost("create")]
        [AllowAnonymous]
        public IActionResult Create([FromBody] OrderDetailRequestDTO request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = _service.CreateOrderDetail(request);
                if (!result)
                    return StatusCode(500, new { message = "Không thể tạo OrderDetail." });
                var createdOrderDetail = _service.GetOrderDetailsByOrderId(request.OrderId);
                return Ok(new 
                {
                    createdOrderDetail,
                    message = "Tạo thành công."
                }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message 
                });
            }
        }

        [HttpPut("update/{id}")]
        [AllowAnonymous]
        public IActionResult Update(Guid id, [FromBody] OrderDetailRequestDTO request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_service.UpdateOrderDetail(id, request))
                return NotFound("Order detail not found or an error occurred while updating.");

            return Ok("Order detail updated successfully.");
        }

        [HttpDelete("delete/{id}")]
        [AllowAnonymous]
        public IActionResult Delete(Guid id)
        {
            if (!_service.DeleteOrderDetail(id))
                return NotFound("Order detail not found or an error occurred while deleting.");

            return Ok("Order detail deleted successfully.");
        }
    }
}
