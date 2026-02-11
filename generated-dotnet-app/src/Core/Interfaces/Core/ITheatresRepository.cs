using BookMyShow.Core.Entities.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Core.Interfaces.Core
{
    public interface ITheatresRepository
    {
        Task<Theatres?> GetByIdAsync(long id);
        Task<IEnumerable<Theatres>> GetAllAsync();
        Task<(IEnumerable<Theatres>, int)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending,
            Dictionary<string, object>? filters);

        Task<Theatres> AddAsync(Theatres entity);
        Task UpdateAsync(Theatres entity);
        Task DeleteAsync(long id);
    }
}
