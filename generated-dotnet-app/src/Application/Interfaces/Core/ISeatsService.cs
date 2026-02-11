using BookMyShow.Application.DTOs.Core.Seats;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Application.Interfaces.Core
{
    public interface ISeatsService
    {
        Task<Result<Seat>> GetByIdAsync(long id);
        Task<Result<IEnumerable<Seat>>> GetByScreenIdAsync(long screenId);
        Task<Result<(IEnumerable<Seat> Items, int TotalCount)>> GetPagedAsync(
            int pageNumber, 
            int pageSize, 
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDescending = false,
            Dictionary<string, object>? filters = null);
        Task<Result<Seat>> CreateAsync(CreateSeatDto dto);
        Task<Result> UpdateAsync(long id, UpdateSeatDto dto);
        Task<Result> DeleteAsync(long id);
    }
}