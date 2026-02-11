using BookMyShow.Application.DTOs.Core.Movies;
using BookMyShow.Core.Common;
using BookMyShow.Core.Entities.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Application.Interfaces.Core
{
    public interface IMoviesService
    {
        Task<Result<Movies>> GetByIdAsync(long id);
        Task<Result<IEnumerable<Movies>>> GetAllAsync();
        Task<Result<(IEnumerable<Movies>, int)>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending);

        Task<Result<Movies>> CreateAsync(CreateMoviesDto dto);
        Task<Result> UpdateAsync(long id, UpdateMoviesDto dto);
        Task<Result> DeleteAsync(long id);
    }
}
