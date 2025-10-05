using AutoMapper;
using HoshiVibe.Entities.DTO.ModelRequests.Product;
using HoshiVibe.Entities.DTO.ModelRequests.User;
using HoshiVibe.Entity.DTO.ModelDTO;
using HoshiVibe.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HoshiVibe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;
        public ProductController( ProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }


        [HttpGet("search")]
        [AllowAnonymous]
        public IActionResult SearchProducts([FromQuery] string? query)
        {
            var products = _productService.Search(query ?? string.Empty);
            return Ok(products);
        }

        [HttpGet("{productId}")]
        [AllowAnonymous]
        public IActionResult GetProductById(Guid productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                return NotFound("Sản phẩm không tồn tại.");
            return Ok(product);
        }


        [HttpPost("create")]
        [AllowAnonymous]
        public IActionResult Create([FromBody] ProductRequestDTO request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_productService.CreateProduct(request, out var product))
                return Conflict("Đã có lỗi xảy ra!");


            return Ok(new
            {
                Message = "Tạo mới thành công."
            });
        }

        [HttpPut("update/{productId}")]
        [AllowAnonymous]
        public IActionResult Update(Guid productId, [FromBody] ProductRequestDTO dto)
        {
            if (dto == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var success = _productService.UpdateProduct(productId, dto);
            if (!success)
                return NotFound("Sản phẩm không tồn tại hoặc cập nhật thất bại.");

            return Ok(new
            {
                Message = "Cập nhật thành công."
            });
        }

        [HttpDelete("delete/{productId}")]
        [AllowAnonymous]
        public IActionResult Delete(Guid productId) {

            var success = _productService.DeleteProduct(productId);
            if (!success)
                return NotFound(new { Message = "Không tìm thấy sản phẩm để xóa." });

            return Ok(new { Message = "Xóa sản phẩm thành công." });

        }
    }
}
