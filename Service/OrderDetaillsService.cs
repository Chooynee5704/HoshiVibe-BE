using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests.OderProcess;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Entity.DTO.ModelDTO;
using HoshiVibe.Entity.Model;
using HoshiVibe.Repositories;
using HoshiVibe.Repository;

namespace HoshiVibe.Service
{
    public class OrderDetaillsService
    {
        private readonly OrderDetailsRepository _orderDetailsRepository;  
        private readonly IMapper _mapper;

        public OrderDetaillsService(OrderDetailsRepository orderDetaillsRepository, IMapper mapper)
        {
            _orderDetailsRepository = orderDetaillsRepository;
            _mapper = mapper;
        }

        public ICollection<OrderDetailDTO>? GetOrderDetailsByOrderId(string orderId)
        {
            var orderDetails = _orderDetailsRepository.GetOrderDetailsByOrderId(orderId);
            if (orderDetails == null)
                return null;
            return _mapper.Map<ICollection<OrderDetailDTO>>(orderDetails);
        }

        public bool CreateOrderDetail(OrderDetailRequestDTO request)
        {
            var orderDetail = _mapper.Map<OrderDetail>(request);
            return _orderDetailsRepository.CreateOrderDetail(orderDetail);
        }

        public bool UpdateOrderDetail(Guid id, OrderDetailRequestDTO request)
        {
            var existingOrderDetail = _orderDetailsRepository.GetOrderDetailById(id);
            if (existingOrderDetail == null ) return false;
            var updatedOrderDetail = _mapper.Map(request, existingOrderDetail);
            return _orderDetailsRepository.UpdateOrderDetail(updatedOrderDetail);

        }

        public bool DeleteOrderDetail(Guid id)
        {
            var existingOrderDetail = _orderDetailsRepository.GetOrderDetailById(id);
            if (existingOrderDetail == null)
                return false;
            return _orderDetailsRepository.DeleteOrderDetail(existingOrderDetail);
        }
    }
}
