using HoshiVibe.Entities.DTO.ModelRequests.CustomDesign;
using HoshiVibe.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HoshiVibe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomDesignController : ControllerBase
    {
        private readonly CustomDesignService _service;

        public CustomDesignController(CustomDesignService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            try
            {
                var list = await _service.GetAllAsync(ct);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            try
            {
                var entity = await _service.GetByIdAsync(id, ct);
                if (entity == null) return NotFound(new { message = "Custom design not found" });
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId, CancellationToken ct)
        {
            try
            {
                var list = await _service.GetByUserIdAsync(userId, ct);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomDesignRqDTO rq, CancellationToken ct)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var created = await _service.CreateAsync(rq, ct);
                return CreatedAtAction(nameof(GetById), new { id = created.CustomDesign_Id }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomDesignRqDTO rq, CancellationToken ct)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var updated = await _service.UpdateAsync(id, rq, ct);
                if (!updated) return NotFound(new { message = "Custom design not found" });
                return Ok(new { message = "Updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id, ct);
                if (!deleted) return NotFound(new { message = "Custom design not found" });
                return Ok(new { message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
