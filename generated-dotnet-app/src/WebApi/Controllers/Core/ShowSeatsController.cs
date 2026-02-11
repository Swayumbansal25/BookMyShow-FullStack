using BookMyShow.Application.Interfaces.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookMyShow.WebApi.Controllers.Core
{
    [ApiController]
    [Route("api/Core/[controller]")]
    public class ShowSeatsController : ControllerBase
    {
        private readonly IShowSeatsService _service;

        public ShowSeatsController(IShowSeatsService service)
        {
            _service = service;
        }

        [HttpGet("show/{showId}")]
        public async Task<IActionResult> GetByShow(long showId)
        {
            var result = await _service.GetByShowIdAsync(showId);
            return Ok(result.Value);
        }

        [HttpPost("initialize")]
        public async Task<IActionResult> Initialize(long showId, long screenId, decimal price = 0)
        {
            var result = await _service.InitializeShowSeatsAsync(showId, screenId, price);
            if (!result.IsSuccess) return BadRequest(result.Error);
            
            return Ok("Show seats initialized successfully");
        }
    }
}