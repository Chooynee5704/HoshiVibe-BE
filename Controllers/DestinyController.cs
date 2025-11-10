using Microsoft.AspNetCore.Mvc;
using HoshiVibe.Service;
using HoshiVibe.Entities.DTO.ModelRequests.User;

namespace HoshiVibe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DestinyController : Controller
    {
        private readonly DestinyService _destinyService;
        public DestinyController(DestinyService destinyService)
        {
            _destinyService = destinyService;
        }
        [HttpGet("all")]
        public IActionResult GetAllDestinies()
        {
            var destinies = _destinyService.GetAllDestinies();
            return Ok(destinies);
        }
        [HttpGet("{id}")]
        public IActionResult GetDestinyById(int id)
        {
            var destiny = _destinyService.GetDestinyById(id);
            if (destiny == null)
            {
                return NotFound("Destiny not found.");
            }
            return Ok(destiny);
        }
        [HttpPut("update{id}")]
        public IActionResult UpdateDestiny(int id, [FromBody] DestinyUpdateDTO destiny)
        {
            var result = _destinyService.UpdateDestiny(id, destiny);
            if (!result)
            {
                return NotFound("Destiny not found or update failed.");
            }
            return Ok("Destiny updated successfully.");
        }
    }
}
