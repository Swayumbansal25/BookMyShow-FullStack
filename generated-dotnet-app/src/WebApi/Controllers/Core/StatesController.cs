using BookMyShow.Application.DTOs.Core.States;
using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Core.Common;
using BookMyShow.Core.Entities.Core;
using BookMyShow.WebApi.DTOs.Common;
using BookMyShow.WebApi.DTOs.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.WebApi.Controllers.Core
{
    [ApiController]
    [Route("api/Core/[controller]")]
    public class StatesController : ControllerBase
    {
        private readonly IStatesService _service;

        public StatesController(IStatesService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.IsFailure ? NotFound(result.Error!.Message) : Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result.Value);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] StatesFilterRequest request)
        {
            var result = await _service.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.SortBy,
                request.SortDescending,
                request.GetFilters());

            var (items, total) = result.Value;
            return Ok(new PagedResponse<States>(items, total, request.PageNumber, request.PageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStatesDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = result.Value!.StateId }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, UpdateStatesDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.IsFailure ? BadRequest(result.Error!.Message) : NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id);
            return result.IsFailure ? NotFound(result.Error!.Message) : NoContent();
        }
    }
}
