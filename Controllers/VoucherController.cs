using AutoMapper;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Entity.DTO.ModelDTO;
using HoshiVibe.Service;
using Microsoft.AspNetCore.Mvc;

namespace HoshiVibe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly VoucherService _voucherService;
        private readonly IMapper _mapper;

        public VoucherController(VoucherService voucherService, IMapper mapper)
        {
            _voucherService = voucherService;
            _mapper = mapper;
        }

        [HttpPost("validate")]
        public async Task<ActionResult<ValidateVoucherResponseDTO>> ValidateVoucher([FromBody] ValidateVoucherRequestDTO request)
        {
            var voucher = await _voucherService.ValidateVoucherCode(request.Code);

            if (voucher == null)
            {
                return Ok(new ValidateVoucherResponseDTO
                {
                    IsValid = false,
                    Message = "Mã giảm giá không hợp lệ hoặc đã hết hạn",
                    Voucher = null
                });
            }

            var voucherDTO = _mapper.Map<VoucherDTO>(voucher);

            return Ok(new ValidateVoucherResponseDTO
            {
                IsValid = true,
                Message = "Mã giảm giá hợp lệ",
                Voucher = voucherDTO
            });
        }

        [HttpGet]
        public async Task<ActionResult<List<VoucherDTO>>> GetAllVouchers()
        {
            var vouchers = await _voucherService.GetAllVouchers();
            var voucherDTOs = _mapper.Map<List<VoucherDTO>>(vouchers);
            return Ok(voucherDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<VoucherDTO>> CreateVoucher([FromBody] CreateVoucherDTO createVoucherDTO)
        {
            var voucher = new Voucher
            {
                Voucher_Id = Guid.NewGuid(),
                Code = createVoucherDTO.Code,
                VoucherName = createVoucherDTO.VoucherName,
                DiscountAmount = createVoucherDTO.DiscountAmount,
                UseTime = createVoucherDTO.UseTime,
                UsedCount = 0,
                StartDate = createVoucherDTO.StartDate,
                EndDate = createVoucherDTO.EndDate,
                IsActive = true
            };

            var createdVoucher = await _voucherService.CreateVoucher(voucher);
            var voucherDTO = _mapper.Map<VoucherDTO>(createdVoucher);

            return CreatedAtAction(nameof(GetAllVouchers), new { id = voucherDTO.Voucher_Id }, voucherDTO);
        }
    }

    // DTOs for validation
    public class ValidateVoucherRequestDTO
    {
        public required string Code { get; set; }
    }

    public class ValidateVoucherResponseDTO
    {
        public bool IsValid { get; set; }
        public string? Message { get; set; }
        public VoucherDTO? Voucher { get; set; }
    }

    public class CreateVoucherDTO
    {
        public required string Code { get; set; }
        public required string VoucherName { get; set; }
        public required decimal DiscountAmount { get; set; }
        public int UseTime { get; set; } = 10;
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
    }
}
