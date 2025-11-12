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
        private readonly OrderService _orderService;

        public OrderDetailController(OrderDetaillsService service, OrderService orderService)
        {
            _service = service;
            _orderService = orderService;
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
        [AllowAnonymous]
        public IActionResult Create([FromBody] OrderDetailRequestDTO request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Get userId from JWT token
                var userIdClaim = User.FindFirst("userId");
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return Unauthorized("User not authenticated. Please login.");
                }

                // If OrderId not provided, try to get user's pending order or create new one
                if (string.IsNullOrEmpty(request.OrderId))
                {
                    var pendingOrder = _orderService.GetUserPendingOrder(userId);
                    
                    if (pendingOrder != null)
                    {
                        request.OrderId = pendingOrder.Order_Id;
                    }
                    else
                    {
                        // Create a new pending order
                        var orderRequest = new OrderRequestDTO
                        {
                            User_Id = userId,
                            TotalPrice = 0,
                            DiscountAmount = 0,
                            FinalPrice = 0,
                            Status = "Pending",
                            OrderDate = DateTime.UtcNow
                        };

                        var newOrder = _orderService.CreateOrder(orderRequest);
                        if (newOrder == null)
                        {
                            return StatusCode(500, new { message = "Không thể tạo đơn hàng mới." });
                        }

                        request.OrderId = newOrder.Order_Id;
                    }
                }

                // Check if product already exists in the order
                var existingOrderDetails = _service.GetOrderDetailsByOrderId(request.OrderId);
                var existingDetail = existingOrderDetails?.FirstOrDefault(od => 
                    (request.ProductId.HasValue && od.ProductId == request.ProductId) ||
                    (request.CProduct_Id.HasValue && od.CProductId == request.CProduct_Id));

                if (existingDetail != null)
                {
                    // Update quantity of existing order detail
                    var updateRequest = new OrderDetailRequestDTO
                    {
                        OrderId = request.OrderId,
                        ProductId = existingDetail.ProductId,
                        CProduct_Id = existingDetail.CProductId,
                        Quantity = existingDetail.Quantity + request.Quantity,
                        UnitPrice = request.UnitPrice,
                        Discount = request.Discount
                    };

                    var updateResult = _service.UpdateOrderDetail(existingDetail.OrderDetailId, updateRequest);
                    if (!updateResult)
                        return StatusCode(500, new { message = "Không thể cập nhật số lượng." });

                    var updatedOrderDetails = _service.GetOrderDetailsByOrderId(request.OrderId);
                    return Ok(new 
                    {
                        createdOrderDetail = updatedOrderDetails,
                        orderId = request.OrderId,
                        message = "Đã cập nhật số lượng sản phẩm trong giỏ hàng."
                    });
                }
                else
                {
                    // Create new order detail
                    var result = _service.CreateOrderDetail(request);
                    if (!result)
                        return StatusCode(500, new { message = "Không thể tạo OrderDetail." });
                    
                    var createdOrderDetail = _service.GetOrderDetailsByOrderId(request.OrderId);
                    return Ok(new 
                    {
                        createdOrderDetail,
                        orderId = request.OrderId,
                        message = "Tạo thành công."
                    });
                }
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
