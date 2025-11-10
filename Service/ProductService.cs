using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests.Product;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Entity.DTO.ModelDTO;
using HoshiVibe.Repositories;
using Microsoft.AspNetCore.Identity;

namespace HoshiVibe.Service
{
    public class ProductService
    {
        private readonly ProductRepository _productRepo;
        private readonly IMapper _mapper;


        public ProductService(ProductRepository produdctRepo, IMapper mapper) {
            _productRepo = produdctRepo;
            _mapper = mapper;
        }

        public List<Product> Search(string keyword) {
            return _productRepo.Search(keyword).ToList();
        }
        public List<ProductDTO> GetProposedProducts(string destiny) {
            if (string.IsNullOrWhiteSpace(destiny))
                throw new ArgumentException("Destiny is required");

            var products = _productRepo.GetProposedProducts(destiny);
            return _mapper.Map<List<ProductDTO>>(products);
        }
        public bool CreateProduct(ProductRequestDTO dto, out Product? product) {

            product = null;
            product = _mapper.Map<Product>(dto);
            product.Product_Id = Guid.NewGuid();

            return _productRepo.CreateProduct(product);
        }
        public ProductDTO? GetProductById(Guid id) {

            var product = _productRepo.GetProductById(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public bool UpdateProduct(Guid productId, ProductRequestDTO dto)
        {
            var product = _productRepo.GetProductById(productId);
            if (product == null) return false;

            // Cập nhật entity từ DTO
            _mapper.Map(dto, product);

            return _productRepo.UpdateProduct(product);
        }



        public bool DeleteProduct(Guid productId) {
            var product = _productRepo.GetProductById(productId);
            if (product == null) return false;

            return _productRepo.DeleteProduct(product);
        }
    }
}
