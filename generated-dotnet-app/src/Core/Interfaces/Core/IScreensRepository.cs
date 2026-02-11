using BookMyShow.Core.Entities.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Core.Interfaces.Core
{
    public interface IScreensRepository
    {
        Task<Screen?> GetByIdAsync(long id);
        Task<IEnumerable<Screen>> GetAllAsync();

        Task<(IEnumerable<Screen>, int)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm);

        Task<Screen> AddAsync(Screen entity);
        Task UpdateAsync(Screen entity);
    }
}
