using BookMyShow.Application.DTOs.Core.Cities;
using BookMyShow.Core.Common;
using BookMyShow.Core.Entities.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Application.Interfaces.Core
{
    public interface ICitiesService
    {
        Task<Result<Cities>> GetByIdAsync(long id);
        Task<Result<IEnumerable<Cities>>> GetAllAsync();
        Task<Result<(IEnumerable<Cities>, int)>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending,
            Dictionary<string, object>? filters);

        Task<Result<Cities>> CreateAsync(CreateCitiesDto dto);
        Task<Result> UpdateAsync(long id, UpdateCitiesDto dto);
        Task<Result> DeleteAsync(long id);
    }
}
