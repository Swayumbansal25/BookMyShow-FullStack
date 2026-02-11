using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Application.DTOs.Core.Bookings;
using BookMyShow.Core.Entities.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.WebApi.Controllers.Core
{
    [ApiController]
    [Route("api/Core/[controller]")]
    [Produces("application/json")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingsService _service;

        public BookingsController(IBookingsService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create a new booking and reserve seats
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var result = await _service.CreateBookingAsync(dto);
            if (result.IsFailure) return BadRequest(result.Error.Message);
            
            return Ok(result.Value);
        }

        /// <summary>
        /// Get a specific booking by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result.IsFailure) return NotFound(result.Error.Message);
            
            return Ok(result.Value);
        }

        /// <summary>
        /// Get all bookings for a specific user
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(long userId)
        {
            var result = await _service.GetByUserIdAsync(userId);
            if (result.IsFailure) return BadRequest(result.Error.Message);
            
            return Ok(result.Value);
        }

        /// <summary>
        /// Update the status of a booking (e.g., to Confirm or Cancel)
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(long id, [FromBody] string status)
        {
            var result = await _service.UpdateStatusAsync(id, status);
            if (result.IsFailure) return NotFound(result.Error.Message);
            
            return NoContent();
        }
    }
}