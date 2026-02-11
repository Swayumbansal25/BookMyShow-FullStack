using BookMyShow.Application.DTOs.Core.Theatres;
using BookMyShow.Core.Common;
using BookMyShow.Core.Entities.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Application.Interfaces.Core
{
    public interface ITheatresService
    {
        Task<Result<Theatres>> GetByIdAsync(long id);
        Task<Result<IEnumerable<Theatres>>> GetAllAsync();
        Task<Result<(IEnumerable<Theatres>, int)>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending,
            Dictionary<string, object>? filters);

        Task<Result<Theatres>> CreateAsync(CreateTheatresDto dto);
        Task<Result> UpdateAsync(long id, UpdateTheatresDto dto);
        Task<Result> DeleteAsync(long id);
    }
}
