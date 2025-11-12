using HoshiVibe.DB;
using HoshiVibe.Entities.DTO.ModelRequests.OderProcess;
using HoshiVibe.Entities.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace HoshiVibe.Repositories
{
    public class OrderDetailsRepository
    {
        DataContext _context;
        public OrderDetailsRepository( DataContext context) {
            _context = context;
        }
        public OrderDetail? GetOrderDetailById(Guid id) {
            return _context.OrderDetails
                .Include(od => od.Product)
                .FirstOrDefault(od => od.OrderDetail_Id == id);
        }

        public ICollection<OrderDetail> GetOrderDetailsByOrderId(string id) {
            return _context.OrderDetails
                .Include(od => od.Product)
                .Include(od => od.CustomDesign)
                .Where(od => od.OrderId == id)
                .OrderBy(od => od.OrderDetail_Id)
                .ToList();
        }

        public bool CreateOrderDetail( OrderDetail orderDetail ) {
            _context.OrderDetails.Add(orderDetail);
            return Save();
        }
        public bool UpdateOrderDetail( OrderDetail orderDetail ) {
            _context.OrderDetails.Update(orderDetail);
            return Save();
        }
        public bool DeleteOrderDetail( OrderDetail orderDetail ) {
            _context.OrderDetails.Remove(orderDetail);
            return Save();
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }


    }
}
