using HoshiVibe.Service;
using HoshiVibe.Entities.DTO.ModelRequests.Product;
using Microsoft.AspNetCore.Mvc;

namespace SalesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharmsController : ControllerBase
    {
        private readonly CharmService _svc;

        public CharmsController(CharmService svc)
        {
            _svc = svc;
        }

        // GET: api/charms
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? q, CancellationToken ct)
        {
            if (!string.IsNullOrWhiteSpace(q))
            {
                var result = await _svc.SearchAsync(q, ct);
                return Ok(result);
            }
            var list = await _svc.GetAllAsync(ct);
            return Ok(list);
        }

        // GET: api/charms/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
        {
            var dto = await _svc.GetByIdAsync(id, ct);
            return dto == null ? NotFound() : Ok(dto);
        }

        // POST: api/charms
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomPdRqDTO rq, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _svc.CreateAsync(rq, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.CProduct_Id }, created);
        }

        // PUT: api/charms/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CustomPdRqDTO rq, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var ok = await _svc.UpdateAsync(id, rq, ct);
            return ok ? NoContent() : NotFound();
        }

        // DELETE: api/charms/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
        {
            var ok = await _svc.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }
    }
}
