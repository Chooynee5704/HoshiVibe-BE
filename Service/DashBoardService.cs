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

        public object GetTopSellingProducts(int topN)
        {
            var productSales = new Dictionary<Guid, int>();
            var orders = _orderRepository.GetAllOrders();
            foreach (var order in orders)
            {
                foreach (var detail in order.OrderDetails)
                {
                    if (detail.ProductId.HasValue)
                    {
                        var productId = detail.ProductId.Value;
                        if (productSales.ContainsKey(productId))
                        {
                            productSales[productId] += detail.Quantity;
                        }
                        else
                        {
                            productSales[productId] = detail.Quantity;
                        }
                    }
                }
            }
            var topSellingProducts = productSales
                .OrderByDescending(ps => ps.Value)
                .Take(topN)
                .Select(ps => new
                {
                    ProductId = ps.Key,
                    TotalSold = ps.Value
                })
                .ToList();
            return topSellingProducts;
        }
    }
}
