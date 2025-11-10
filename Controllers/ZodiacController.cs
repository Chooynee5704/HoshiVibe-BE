using HoshiVibe.Entities.DTO.ModelRequests.User;
using HoshiVibe.Service;
using Microsoft.AspNetCore.Mvc;

namespace HoshiVibe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZodiacController : Controller
    {
        private readonly ZodiacService _zodiacService;
        public ZodiacController(ZodiacService zodiacService)
        {
            _zodiacService = zodiacService;
        }

        [HttpGet("all")]
        public IActionResult GetAllZodiacs()
        {
            var zodiacs = _zodiacService.GetAllZodiacs();
            return Ok(zodiacs);
        }

        [HttpGet("{id}")]
        public IActionResult GetZodiacById(int id)
        {
            var zodiac = _zodiacService.GetZodiacById(id);
            if (zodiac == null)
            {
                return NotFound("Zodiac not found.");
            }
            return Ok(zodiac);
        }

        [HttpPut("update{id}")]
        
        public IActionResult UpdateZodiac(int id, [FromBody] ZodiacUpdateDTO zodiac)
        {
            var result = _zodiacService.UpdateZodiac(id, zodiac);
            if (!result)
            {
                return NotFound("Zodiac not found or update failed.");
            }
            return Ok("Zodiac updated successfully.");
        }

    }
}
