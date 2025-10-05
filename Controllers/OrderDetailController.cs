using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests.OderProcess;
using HoshiVibe.Service;
using Microsoft.AspNetCore.Mvc;

namespace HoshiVibe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDetailController : Controller
    {
        private readonly OrderDetaillsService _service;

        public OrderDetailController (OrderDetaillsService service)
        {
            _service = service;
        }

        [HttpGet("order/{orderId}")]
        public IActionResult GetOrderDetailsByOrderId(string orderId)
        {
            var orderDetails = _service.GetOrderDetailsByOrderId(orderId);

            if (orderDetails == null || !orderDetails.Any())
                return NotFound("No order details found for the specified order ID.");

            return Ok(orderDetails);
        }
        [HttpPost("create")]
        public IActionResult Create([FromBody] OrderDetailRequestDTO request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_service.CreateOrderDetail(request))
                return Conflict("An error occurred while creating the order detail.");

            return Ok("Order detail created successfully.");
        }

        [HttpPut("update/{id}")]
        public IActionResult Update(Guid id, [FromBody] OrderDetailRequestDTO request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_service.UpdateOrderDetail(id, request))
                return NotFound("Order detail not found or an error occurred while updating.");

            return Ok("Order detail updated successfully.");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            if (!_service.DeleteOrderDetail(id))
                return NotFound("Order detail not found or an error occurred while deleting.");

            return Ok("Order detail deleted successfully.");
        }
    }
}
