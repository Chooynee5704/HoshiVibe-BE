using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests.OderProcess;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Entity.DTO.ModelDTO;
using HoshiVibe.Repository;

namespace HoshiVibe.Service
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderService(OrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public bool MarkPaid(string orderId, long expectedAmountVnd, string method = "VNPAY")
        {
            var order = _orderRepository.GetOrderById(orderId);
            if (order == null) return false;

            // Nếu không khớp tiền thì từ chối
            long finalPriceRounded = (long)Math.Round(order.FinalPrice, 0);
            if (finalPriceRounded != expectedAmountVnd) return false;

            // Idempotent: nếu đã Paid thì coi như thành công
            if (string.Equals(order.Status, "Paid", StringComparison.OrdinalIgnoreCase))
                return true;

            // Nếu đang Pending mới chuyển sang Paid (tuỳ chính sách, bạn có thể cho từ Failed->Paid nếu IPN đến sau)
            if (!string.Equals(order.Status, "Pending", StringComparison.OrdinalIgnoreCase))
                return false;

            // Cập nhật trạng thái
            order.Status = "Paid";

            // Ghi log thanh toán (nếu repo hỗ trợ add payment)
            _orderRepository.AddPayment(new Payment
            {
                Order_Id = order.Order_Id,
                Amount = expectedAmountVnd,
                PaymentDate = DateTime.UtcNow,
                Status = "Success",
                PaymentMethod = method
            });

            return _orderRepository.Save(); // SaveChanges trả bool
        }
        public ICollection<OrderDTO> GetAllOrders()
        {
            var orders = _orderRepository.GetAllOrders();
            return _mapper.Map<ICollection<OrderDTO>>(orders);
        }
        public OrderDTO? GetOrderById(string id)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order == null)
                return null;
            return _mapper.Map<OrderDTO>(order);
        }
        public OrderDTO? GetOrderByUserId(Guid id)
        {
            var order = _orderRepository.GetOrderByUserId(id);
            if (order == null)
                return null;
            return _mapper.Map<OrderDTO>(order);
        }

        public ICollection<OrderDTO> GetAllOrdersByUserId(Guid userId)
        {
            var orders = _orderRepository.GetAllOrdersByUserId(userId);
            return _mapper.Map<ICollection<OrderDTO>>(orders);
        }

        public ICollection<OrderDTO> GetPendingOrders()
        {
            var orders = _orderRepository.GetPendingOrders();
            return _mapper.Map<ICollection<OrderDTO>>(orders);
        }

        public OrderDTO? GetUserPendingOrder(Guid userId)
        {
            var order = _orderRepository.GetUserPendingOrder(userId);
            if (order == null)
                return null;
            return _mapper.Map<OrderDTO>(order);
        }
        public OrderDTO? CreateOrder(OrderRequestDTO orderRequest)
        {
            var order = _mapper.Map<Order>(orderRequest);
            order.Order_Id = new Random().Next(100000, 999999).ToString();
            order.OrderDate = DateTime.UtcNow;
            order.Status = "Pending";
            
            if (!_orderRepository.CreateOrder(order))
                return null;
            
            return _mapper.Map<OrderDTO>(order);
        }
        public bool UpdateOrder(string id, OrderRequestDTO orderRequest)
        {
            var existingOrder = _orderRepository.GetOrderById(id);
            if (existingOrder == null)
                return false;
            var updatedOrder = _mapper.Map(orderRequest, existingOrder);
            return _orderRepository.UpdateOrder(updatedOrder);
        }

        public bool UpdateShippingStatus(string orderId, string shippingStatus)
        {
            var order = _orderRepository.GetOrderById(orderId);
            if (order == null)
                return false;

            // Validate shipping status
            var validStatuses = new[] { "Pending", "Shipping", "Delivered", "PickedUp" };
            if (!validStatuses.Contains(shippingStatus, StringComparer.OrdinalIgnoreCase))
                return false;

            order.ShippingStatus = shippingStatus;
            return _orderRepository.UpdateOrder(order);
        }

        public bool DeleteOrder(string id)
        {
            var existingOrder = _orderRepository.GetOrderById(id);
            if (existingOrder == null)
                return false;
            return _orderRepository.DeleteOrder(existingOrder);
        }
    }
}
