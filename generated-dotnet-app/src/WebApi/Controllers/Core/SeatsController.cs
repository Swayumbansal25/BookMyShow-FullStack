using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Application.DTOs.Core.Seats;
using BookMyShow.Core.Entities.Core;
using BookMyShow.WebApi.DTOs.Common;
using BookMyShow.WebApi.DTOs.Core;
using Microsoft.AspNetCore.Mvc;

namespace BookMyShow.WebApi.Controllers.Core
{
    [ApiController]
    [Route("api/Core/[controller]")]
    [Produces("application/json")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatsService _service;

        public SeatsController(ISeatsService service)
        {
            _service = service;
        }

        // 1. GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result.IsFailure) return NotFound(result.Error.Message);
            return Ok(result.Value);
        }

        // 2. GET ALL BY SCREEN
        [HttpGet("screen/{screenId}")]
        public async Task<IActionResult> GetByScreen(long screenId)
        {
            var result = await _service.GetByScreenIdAsync(screenId);
            return Ok(result.Value);
        }

        // 3. GET PAGED (Now added to Controller)
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] UsersFilterRequest request)
        {
            // Note: Using UsersFilterRequest because it has the paging/sorting properties 
            // you already defined. You can use it here too!
            var result = await _service.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.SortBy,
                request.SortDescending,
                request.GetFilters());

            if (result.IsFailure) return BadRequest(result.Error.Message);

            var (items, totalCount) = result.Value;
            var response = new PagedResponse<Seat>(
                items,
                totalCount,
                request.PageNumber,
                request.PageSize);

            return Ok(response);
        }

        // 4. POST (CREATE)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSeatDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.CreateAsync(dto);
            return Ok(result.Value);
        }

        // 5. PUT (UPDATE) - Now added
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateSeatDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.UpdateAsync(id, dto);
            if (result.IsFailure) return NotFound(result.Error.Message);
            return NoContent();
        }

        // 6. DELETE - Now added
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id);
            if (result.IsFailure) return BadRequest(result.Error.Message);
            return NoContent();
        }
    }
}