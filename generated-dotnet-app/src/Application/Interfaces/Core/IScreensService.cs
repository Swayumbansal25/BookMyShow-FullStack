using BookMyShow.Application.DTOs.Core.Screens;
using BookMyShow.Core.Common;
using BookMyShow.Core.Entities.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Application.Interfaces.Core
{
    public interface IScreensService
    {
        Task<Result<Screen>> GetByIdAsync(long id);

        Task<Result<(IEnumerable<Screen>, int)>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm);

        Task<Result<Screen>> CreateAsync(CreateScreenDto dto);

        Task<Result> UpdateAsync(long id, UpdateScreenDto dto);

        // ✅ Soft delete
        Task<Result> DeactivateAsync(long id);
    }
}
