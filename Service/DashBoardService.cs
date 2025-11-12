using HoshiVibe.Repositories;
using HoshiVibe.Repository;

namespace HoshiVibe.Service
{
    public class DashBoardService
    {
        private readonly ProductRepository _productRepository;
        private readonly OrderRepository _orderRepository;
        private readonly UserRepository _userRepository;
        private readonly CartItemRepository _cartItemRepository;
    
        public DashBoardService(ProductRepository productRepository, OrderRepository orderRepository, UserRepository userRepository, CartItemRepository cartItemRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _cartItemRepository = cartItemRepository;
        }

        public int GetTotalProducts()
        {
            return _productRepository.Search("").Count;
        }
        public int GetTotalOrders()
        {
            return _orderRepository.GetAllOrders().Count;
        }
        public int GetTotalUsers()
        {
            return _userRepository.GetAllUsers("Customer").Count;
        }

        public object? GetOrderSummaryByProductId(Guid productId)
        {
            var orders = _orderRepository.GetAllOrders()
                .Where(o => o.OrderDetails.Any(od => od.ProductId == productId))
                .ToList();
            if (orders.Count == 0)
                return null;
            var totalOrders = orders.Count;
            var totalQuantity = orders.Sum(o => o.OrderDetails
                                                .Where(od => od.ProductId == productId)
                                                .Sum(od => od.Quantity));
            var totalRevenue = orders.Sum(o => o.OrderDetails
                                                .Where(od => od.ProductId == productId)
                                                .Sum(od => od.Quantity * od.UnitPrice));
            return new
            {
                TotalOrders = totalOrders,
                TotalQuantity = totalQuantity,
                TotalRevenue = totalRevenue
            };
        }

        public IEnumerable<object> GetMostAddedToCartProducts(int topN)
        {
            var topAdded = _cartItemRepository.GetMostAddedProducts(topN);
            var products = _productRepository.Search("");

            var result = from a in topAdded
                         join p in products on (Guid)a.GetType().GetProperty("ProductId")!.GetValue(a)! equals p.Product_Id
                         select new
                         {
                             p.Product_Id,
                             p.Name,
                             p.ImageUrl,
                             p.Price,
                             TotalAdded = a.GetType().GetProperty("TotalAdded")!.GetValue(a),
                             TimesAdded = a.GetType().GetProperty("TimesAdded")!.GetValue(a)
                         };

            return result;
        }
        public int GetTotalOrdersByMonth(int month, int year)
        {
            return _orderRepository.GetTotalOrdersByMonth(month, year);
        }

        // 🆕 Thống kê theo từng tháng trong năm
        public IEnumerable<object> GetMonthlyOrderStatistics(int year)
        {
            return _orderRepository.GetMonthlyOrderStatistics(year);
        }

        // 🆕 Thống kê theo ngày trong tháng
        public IEnumerable<object> GetDailyRevenueStatistics(int month, int year)
        {
            return _orderRepository.GetDailyRevenueStatistics(month, year);
        }

        // 🆕 Lấy tổng doanh thu
        public decimal GetTotalRevenue()
        {
            return _orderRepository.GetTotalRevenue();
        }

        public object GetTopSellingProducts(int topN)
        {
            var orders = _orderRepository.GetAllOrders();
            var products = _productRepository.Search("");

            var productSales = orders
                .Where(o => o.OrderDetails != null)
                .SelectMany(o => o.OrderDetails!)
                .Where(od => od.ProductId.HasValue)
                .GroupBy(od => od.ProductId!.Value)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantitySold = g.Sum(od => od.Quantity),
                    TotalRevenue = g.Sum(od => od.Quantity * od.UnitPrice - od.Discount)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .Take(topN)
                .ToList();

            var result = from ps in productSales
                         join p in products on ps.ProductId equals p.Product_Id
                         select new
                         {
                             Product_Id = p.Product_Id,
                             Name = p.Name,
                             Category = p.Category ?? "Khác",
                             ImageUrl = p.ImageUrl,
                             TotalQuantitySold = ps.TotalQuantitySold,
                             TotalRevenue = ps.TotalRevenue
                         };

            return result.ToList();
        }
    }
}
