using BookMyShow.Core.Entities.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Core.Interfaces.Core
{
    public interface ISeatsRepository
    {
        Task<Seat?> GetByIdAsync(long id);
        Task<IEnumerable<Seat>> GetByScreenIdAsync(long screenId);
        Task<(IEnumerable<Seat> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, 
            int pageSize, 
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDescending = false,
            Dictionary<string, object>? filters = null);
        Task<Seat> AddAsync(Seat entity);
        Task UpdateAsync(Seat entity);
        Task DeleteAsync(long id);
    }
}