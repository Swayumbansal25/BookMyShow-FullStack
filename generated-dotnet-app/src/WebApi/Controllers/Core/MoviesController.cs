using BookMyShow.Application.DTOs.Core.Movies;
using BookMyShow.Application.Interfaces.Core;
using BookMyShow.WebApi.DTOs.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookMyShow.WebApi.Controllers.Core
{
    [ApiController]
    [Route("api/Core/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _service;

        public MoviesController(IMoviesService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
            => Ok((await _service.GetByIdAsync(id)).Value);

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null)
        {
            var result = await _service.GetPagedAsync(
                pageNumber, pageSize, searchTerm, null, false);

            var (items, total) = result.Value;
            return Ok(new PagedResponse<object>(items, total, pageNumber, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMoviesDto dto)
            => Ok((await _service.CreateAsync(dto)).Value);

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, UpdateMoviesDto dto)
            => Ok(await _service.UpdateAsync(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
            => Ok(await _service.DeleteAsync(id));
    }
}
