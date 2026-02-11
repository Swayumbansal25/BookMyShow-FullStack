using BookMyShow.Core.Common;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Application.DTOs.Core.States;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Application.Interfaces.Core
{
    public interface IStatesService
    {
        Task<Result<States>> GetByIdAsync(long id);
        Task<Result<IEnumerable<States>>> GetAllAsync();
        Task<Result<(IEnumerable<States> Items, int TotalCount)>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDescending = false,
            Dictionary<string, object>? filters = null);

        Task<Result<States>> CreateAsync(CreateStatesDto dto);
        Task<Result> UpdateAsync(long id, UpdateStatesDto dto);
        Task<Result> DeleteAsync(long id);
    }
}
