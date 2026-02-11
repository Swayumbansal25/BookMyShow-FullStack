using BookMyShow.Application.DTOs.Core.Theatres;
using BookMyShow.Application.Interfaces.Core;
using BookMyShow.WebApi.DTOs.Common;
using BookMyShow.WebApi.DTOs.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookMyShow.WebApi.Controllers.Core
{
    [ApiController]
    [Route("api/Core/[controller]")]
    public class TheatresController : ControllerBase
    {
        private readonly ITheatresService _service;

        public TheatresController(ITheatresService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
            => Ok((await _service.GetByIdAsync(id)).Value);

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] TheatresFilterRequest req)
        {
            var result = await _service.GetPagedAsync(
                req.PageNumber, req.PageSize, req.SearchTerm,
                null, false, req.GetFilters());

            var (items, total) = result.Value;
            return Ok(new PagedResponse<object>(items, total, req.PageNumber, req.PageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTheatresDto dto)
            => Ok((await _service.CreateAsync(dto)).Value);

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, UpdateTheatresDto dto)
            => Ok(await _service.UpdateAsync(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
            => Ok(await _service.DeleteAsync(id));
    }
}
