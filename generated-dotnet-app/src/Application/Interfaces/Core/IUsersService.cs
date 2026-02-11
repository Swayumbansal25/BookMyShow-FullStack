using BookMyShow.Application.DTOs.Core.Users;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
// AutoMapper is used in the implementation; interface doesn't require it directly

namespace BookMyShow.Application.Interfaces.Core
{
    /// <summary>
    /// Service interface for Users operations
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Get Users by ID
        /// </summary>
        Task<Result<Users>> GetByIdAsync(long id);
        
        /// <summary>
        /// Get all Users entities
        /// </summary>
        Task<Result<IEnumerable<Users>>> GetAllAsync();
        
        /// <summary>
        /// Get paginated Users entities with filtering and sorting
        /// </summary>
        Task<Result<(IEnumerable<Users> Items, int TotalCount)>> GetPagedAsync(
            int pageNumber, 
            int pageSize, 
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDescending = false,
            Dictionary<string, object>? filters = null);
        
        /// <summary>
        /// Create a new Users
        /// </summary>
        Task<Result<Users>> CreateAsync(CreateUsersDto dto);
        
        /// <summary>
        /// Update an existing Users
        /// </summary>
        Task<Result> UpdateAsync(long id, UpdateUsersDto dto);
        
        /// <summary>
        /// Delete a Users by ID
        /// </summary>
        Task<Result> DeleteAsync(long id);
        
        /// <summary>
        /// Check if Users exists
        /// </summary>
        Task<Result<bool>> ExistsAsync(long id);
    }
}