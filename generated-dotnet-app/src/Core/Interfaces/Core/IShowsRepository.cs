using BookMyShow.Core.Entities.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Core.Interfaces.Core
{
    public interface IShowsRepository
    {
        Task<Show?> GetByIdAsync(long id);

        Task<(IEnumerable<Show>, int)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            long? movieId,
            long? screenId);

        Task<Show> AddAsync(Show entity);
        Task UpdateAsync(Show entity);
        Task DeleteAsync(long id);
    }
}
