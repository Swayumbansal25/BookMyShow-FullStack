using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Application.DTOs.Core.Payments;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.WebApi.Controllers.Core
{
    [ApiController]
    [Route("api/Core/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentsService _service;

        public PaymentsController(IPaymentsService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Process(ProcessPaymentDto dto)
        {
            var result = await _service.ProcessPaymentAsync(dto);
            if (result.IsFailure) return BadRequest(result.Error.Message);
            return Ok(result.Value);
        }

        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetByBooking(long bookingId)
        {
            var result = await _service.GetByBookingIdAsync(bookingId);
            if (result.IsFailure) return NotFound(result.Error.Message);
            return Ok(result.Value);
        }
    }
}