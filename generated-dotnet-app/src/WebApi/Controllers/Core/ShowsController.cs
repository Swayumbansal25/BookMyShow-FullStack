using BookMyShow.Application.DTOs.Core.Shows;
using BookMyShow.Application.Interfaces.Core;
using BookMyShow.WebApi.DTOs.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookMyShow.WebApi.Controllers.Core
{
    [ApiController]
    [Route("api/Core/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowsService _service;

        public ShowsController(IShowsService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var result = await _service.GetByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

       [HttpGet("paged")]
public async Task<IActionResult> GetPaged(
    int pageNumber = 1,
    int pageSize = 10,
    long? movieId = null,
    long? screenId = null)
{
    var result = await _service.GetPagedAsync(pageNumber, pageSize, movieId, screenId);

    var paged = result.Value;

    return Ok(new PagedResponse<object>(
        paged.Items,
        paged.TotalCount,
        paged.PageNumber,
        paged.PageSize));
}


        [HttpPost]
        public async Task<IActionResult> Create(CreateShowDto dto)
        {
            var result = await _service.CreateAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, UpdateShowDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok();
        }
    }
}
