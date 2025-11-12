using HoshiVibe.DB;
using HoshiVibe.Entities.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace HoshiVibe.Repository
{
    public class OrderRepository
    {
        DataContext _context;
        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public bool OrderExists(string id)
        {
            return _context.Orders.Any(u => u.Order_Id == id);
        }

        public void AddPayment(Payment p) => _context.Payments.Add(p);
        public ICollection<Order> GetAllOrders()
        {
            return _context.Orders
                 .Include(o => o.OrderDetails)
                .OrderBy(o => o.OrderDate)
                .ToList();
        }
        public Order? GetOrderById(string id)
        {
            return _context.Orders
                 .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.Order_Id == id);
        }
        public Order? GetOrderByUserId(Guid id)
        {
            return _context.Orders
                 .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.User_Id == id);
        }

        public ICollection<Order> GetAllOrdersByUserId(Guid userId)
        {
            return _context.Orders
                .Include(o => o.OrderDetails!)
                    .ThenInclude(od => od.Product)
                .Where(o => o.User_Id == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public ICollection<Order> GetPendingOrders()
        {
            return _context.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.Status == "Pending")
                .ToList();
        }

        public Order? GetUserPendingOrder(Guid userId)
        {
            return _context.Orders
                .Include(o => o.OrderDetails!)
                    .ThenInclude(od => od.Product)
                .Where(o => o.User_Id == userId && o.Status == "Pending")
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefault();
        }

        public bool CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            return Save();
        }

        public bool UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            return Save();
        }

        public bool DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
            return Save();
        }

        public int GetTotalOrdersByMonth(int month, int year)
        {
            return _context.Orders
                .Where(o => o.OrderDate.Month == month && o.OrderDate.Year == year)
                .Count();
        }

        // 🆕 Lấy thống kê tổng đơn hàng theo tất cả các tháng trong năm (dạng biểu đồ)
        public IEnumerable<object> GetMonthlyOrderStatistics(int year)
        {
            return _context.Orders
                .Where(o => o.OrderDate.Year == year)
                .GroupBy(o => o.OrderDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalOrders = g.Count(),
                    TotalRevenue = g.Sum(o => o.FinalPrice)
                })
                .OrderBy(x => x.Month)
                .ToList();
        }

        // 🆕 Lấy thống kê doanh thu theo ngày trong tháng
        public IEnumerable<object> GetDailyRevenueStatistics(int month, int year)
        {
            return _context.Orders
                .Where(o => o.OrderDate.Month == month && o.OrderDate.Year == year)
                .GroupBy(o => o.OrderDate.Day)
                .Select(g => new
                {
                    Day = g.Key,
                    TotalOrders = g.Count(),
                    TotalRevenue = g.Sum(o => o.FinalPrice)
                })
                .OrderBy(x => x.Day)
                .ToList();
        }

        // 🆕 Lấy tổng doanh thu
        public decimal GetTotalRevenue()
        {
            return _context.Orders
                .Where(o => o.Status == "Paid" || o.Status == "Completed")
                .Sum(o => o.FinalPrice);
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

    }
}
