using BookMyShow.Core.Entities.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Core.Interfaces.Core
{
    public interface IStatesRepository
    {
        Task<States?> GetByIdAsync(long id);
        Task<IEnumerable<States>> GetAllAsync();
        Task<(IEnumerable<States> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDescending = false,
            Dictionary<string, object>? filters = null);

        Task<States> AddAsync(States entity);
        Task UpdateAsync(States entity);
        Task DeleteAsync(long id);
    }
}
