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
        private readonly ProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderDetaillsService(OrderDetailsRepository orderDetaillsRepository, ProductRepository productRepository , IMapper mapper)
        {
            _orderDetailsRepository = orderDetaillsRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public OrderDetailDTO? GetOrderDetailById(Guid id)
        {
            var orderDetail = _orderDetailsRepository.GetOrderDetailById(id);
            if (orderDetail == null)
                return null;
            return _mapper.Map<OrderDetailDTO>(orderDetail);
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
            // Validate that either ProductId or CProduct_Id is provided
            if (!request.ProductId.HasValue && !request.CProduct_Id.HasValue)
            {
                throw new Exception("ProductId hoặc CProduct_Id phải được cung cấp.");
            }

            var orderDetail = _mapper.Map<OrderDetail>(request);
            orderDetail.OrderDetail_Id = Guid.NewGuid();

            // Only validate and update stock for regular products (not custom products)
            if (request.ProductId.HasValue)
            {
                var product = _productRepository.GetProductById(request.ProductId.Value);

                if (product == null)
                {
                    throw new Exception($"Không tìm thấy sản phẩm với ID: {request.ProductId}");
                }

                if (product.Stock < request.Quantity)
                {
                    throw new Exception($"Sản phẩm '{product.Name}' không đủ hàng trong kho. (Còn lại: {product.Stock})");
                }

                product.Stock -= request.Quantity;

                var updatedProduct = _productRepository.UpdateProduct(product);

                if (!updatedProduct)
                {
                    throw new Exception($"Không thể cập nhật tồn kho cho sản phẩm: {product.Name}");
                }
            }

            return _orderDetailsRepository.CreateOrderDetail(orderDetail);
        }

        public bool UpdateOrderDetail(Guid id, OrderDetailRequestDTO request)
        {
            var existingOrderDetail = _orderDetailsRepository.GetOrderDetailById(id);
            if (existingOrderDetail == null ) return false;

            // Calculate quantity difference for stock adjustment
            int quantityDifference = request.Quantity - existingOrderDetail.Quantity;

            // Only adjust stock for regular products (not custom products) and only if quantity changed
            if (request.ProductId.HasValue && quantityDifference != 0)
            {
                var product = _productRepository.GetProductById(request.ProductId.Value);

                if (product == null)
                {
                    throw new Exception($"Không tìm thấy sản phẩm với ID: {request.ProductId}");
                }

                // Check if we have enough stock for increase
                if (quantityDifference > 0 && product.Stock < quantityDifference)
                {
                    throw new Exception($"Sản phẩm '{product.Name}' không đủ hàng trong kho. (Còn lại: {product.Stock})");
                }

                // Adjust stock: decrease if quantity increased, increase if quantity decreased
                product.Stock -= quantityDifference;

                var updatedProduct = _productRepository.UpdateProduct(product);

                if (!updatedProduct)
                {
                    throw new Exception($"Không thể cập nhật tồn kho cho sản phẩm: {product.Name}");
                }
            }

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
