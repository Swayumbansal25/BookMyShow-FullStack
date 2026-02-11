using BookMyShow.Application.DTOs.Core.Shows;
using BookMyShow.Core.Common;
using BookMyShow.Core.Entities.Core;
using System.Threading.Tasks;

namespace BookMyShow.Application.Interfaces.Core
{
    public interface IShowsService
    {
        Task<Result<Show>> GetByIdAsync(long id);

        Task<Result<PagedResult<Show>>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            long? movieId,
            long? screenId);

        Task<Result<Show>> CreateAsync(CreateShowDto dto);
        Task<Result> UpdateAsync(long id, UpdateShowDto dto);
        Task<Result> DeleteAsync(long id);
    }
}
