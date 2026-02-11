using BookMyShow.Core.Entities.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Core.Interfaces.Core
{
    public interface ICitiesRepository
    {
        Task<Cities?> GetByIdAsync(long id);
        Task<IEnumerable<Cities>> GetAllAsync();
        Task<(IEnumerable<Cities>, int)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending,
            Dictionary<string, object>? filters);

        Task<Cities> AddAsync(Cities entity);
        Task UpdateAsync(Cities entity);
        Task DeleteAsync(long id);
    }
}
