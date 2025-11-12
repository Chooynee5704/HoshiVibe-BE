using HoshiVibe.Service;
using Microsoft.AspNetCore.Mvc;

namespace HoshiVibe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashBoardController : Controller
    {
        private readonly DashBoardService _dashBoardService;
        public DashBoardController(DashBoardService dashBoardService)
        {
            _dashBoardService = dashBoardService;
        }

        [HttpGet("stats")]
        public IActionResult GetDashboardStats()
        {
            var totalProducts = _dashBoardService.GetTotalProducts();
            var totalOrders = _dashBoardService.GetTotalOrders();
            var totalUsers = _dashBoardService.GetTotalUsers();
            var totalRevenue = _dashBoardService.GetTotalRevenue();
            var stats = new
            {
                TotalProducts = totalProducts,
                TotalOrders = totalOrders,
                TotalUsers = totalUsers,
                TotalRevenue = totalRevenue
            };
            return Ok(new
            {
                Message = "Tổng các số liệu :",
                Summary = stats
            });

        }

        [HttpGet("get-summary-order{productId}")]
        public IActionResult GetSummaryOrder(Guid productId)
        {
            var summary = _dashBoardService.GetOrderSummaryByProductId(productId);
            if (summary == null)
                return NotFound("No order summary found for the specified product.");
            return Ok(summary);
        }
        [HttpGet("most-added-products")]
        public IActionResult GetMostAddedProducts([FromQuery] int topN = 5)
        {
            var result = _dashBoardService.GetMostAddedToCartProducts(topN);
            return Ok(result);
        }

        [HttpGet("orders-by-month")]
        public IActionResult GetTotalOrdersByMonth([FromQuery] int month, [FromQuery] int year)
        {
            var totalOrders = _dashBoardService.GetTotalOrdersByMonth(month, year);
            return Ok(new
            {
                Message = $"Tổng số đơn hàng trong tháng {month}/{year}",
                TotalOrders = totalOrders
            });
        }

        [HttpGet("monthly-order-stats")]
        public IActionResult GetMonthlyOrderStatistics([FromQuery] int year)
        {
            var data = _dashBoardService.GetMonthlyOrderStatistics(year);
            return Ok(new
            {
                Message = $"Thống kê đơn hàng theo từng tháng của năm {year}",
                Data = data
            });
        }

        [HttpGet("get-top-selling-product{top}")]
        public IActionResult GetTopSellingProducts(int top)
        {
            var topSellingProducts = _dashBoardService.GetTopSellingProducts(top);
            return Ok(topSellingProducts);
        }

        [HttpGet("daily-revenue-stats")]
        public IActionResult GetDailyRevenueStatistics([FromQuery] int month, [FromQuery] int year)
        {
            var data = _dashBoardService.GetDailyRevenueStatistics(month, year);
            return Ok(new
            {
                Message = $"Thống kê doanh thu theo ngày của tháng {month}/{year}",
                Data = data
            });
        }
    }
}
