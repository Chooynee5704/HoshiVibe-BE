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
        public ICollection<OrderDTO> GetPendingOrders()
        {
            var orders = _orderRepository.GetPendingOrders();
            return _mapper.Map<ICollection<OrderDTO>>(orders);
        }
        public bool CreateOrder(OrderRequestDTO orderRequest)
        {

            var order = _mapper.Map<Order>(orderRequest);
            order.Order_Id = new Random().Next(100000, 999999).ToString();
            order.OrderDate = DateTime.UtcNow;
            order.Status = "Pending";
            return _orderRepository.CreateOrder(order);
        }
        public bool UpdateOrder(string id, OrderRequestDTO orderRequest)
        {
            var existingOrder = _orderRepository.GetOrderById(id);
            if (existingOrder == null)
                return false;
            var updatedOrder = _mapper.Map(orderRequest, existingOrder);
            return _orderRepository.UpdateOrder(updatedOrder);
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
