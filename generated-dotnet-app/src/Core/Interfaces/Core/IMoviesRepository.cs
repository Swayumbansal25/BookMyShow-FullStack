using BookMyShow.Core.Entities.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Core.Interfaces.Core
{
    public interface IMoviesRepository
    {
        Task<Movies?> GetByIdAsync(long id);
        Task<IEnumerable<Movies>> GetAllAsync();
        Task<(IEnumerable<Movies>, int)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending);

        Task<Movies> AddAsync(Movies entity);
        Task UpdateAsync(Movies entity);
        Task DeleteAsync(long id);
    }
}
