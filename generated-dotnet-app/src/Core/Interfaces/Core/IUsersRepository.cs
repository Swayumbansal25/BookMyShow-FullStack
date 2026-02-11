using BookMyShow.Core.Entities.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Core.Interfaces.Core
{
    public interface IUsersRepository
    {
        Task<Users?> GetByIdAsync(long id);
        Task<IEnumerable<Users>> GetAllAsync();
        Task<(IEnumerable<Users> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, 
            int pageSize, 
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDescending = false,
            Dictionary<string, object>? filters = null);
        Task<Users> AddAsync(Users entity);
        Task UpdateAsync(Users entity);
        Task DeleteAsync(long id);
    }
}