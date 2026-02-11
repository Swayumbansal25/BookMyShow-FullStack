using BookMyShow.Application.DTOs.Core.Screens;
using BookMyShow.Application.Interfaces.Core;
using BookMyShow.WebApi.DTOs.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookMyShow.WebApi.Controllers.Core
{
    [ApiController]
    [Route("api/core/[controller]")]
    public class ScreensController : ControllerBase
    {
        private readonly IScreensService _service;

        public ScreensController(IScreensService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null)
        {
            var result = await _service.GetPagedAsync(pageNumber, pageSize, searchTerm);
            var (items, total) = result.Value;
            return Ok(new PagedResponse<object>(items, total, pageNumber, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateScreenDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, UpdateScreenDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(long id)
        {
            var result = await _service.DeactivateAsync(id);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
