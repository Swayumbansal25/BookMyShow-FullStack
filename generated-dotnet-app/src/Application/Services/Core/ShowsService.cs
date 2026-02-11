using BookMyShow.Application.DTOs.Core.Shows;
using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Core.Common;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using System.Threading.Tasks;

namespace BookMyShow.Application.Services.Core
{
    public class ShowsService : IShowsService
    {
        private readonly IShowsRepository _showsRepository;

        public ShowsService(IShowsRepository showsRepository)
        {
            _showsRepository = showsRepository;
        }

        public async Task<Result<Show>> GetByIdAsync(long id)
        {
            var show = await _showsRepository.GetByIdAsync(id);

            if (show == null)
                return Result<Show>.Failure("Show not found");

            return Result<Show>.Success(show);
        }

        public async Task<Result<PagedResult<Show>>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            long? movieId,
            long? screenId)
        {
            var (data, totalCount) = await _showsRepository.GetPagedAsync(
                pageNumber,
                pageSize,
                movieId,
                screenId
            );

            var pagedResult = new PagedResult<Show>(
                data,
                totalCount,
                pageNumber,
                pageSize
            );

            return Result<PagedResult<Show>>.Success(pagedResult);
        }

        public async Task<Result<Show>> CreateAsync(CreateShowDto dto)
        {
            var show = new Show
            {
                MovieId = dto.MovieId,
                ScreenId = dto.ScreenId,
                ShowDate = dto.ShowDate, // No conversion needed now
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Price = dto.Price
            };

            var createdShow = await _showsRepository.AddAsync(show);

            return Result<Show>.Success(createdShow);
        }

        public async Task<Result> UpdateAsync(long id, UpdateShowDto dto)
        {
            var show = await _showsRepository.GetByIdAsync(id);

            if (show == null)
                return Result.Failure("Show not found");

            show.MovieId = dto.MovieId;
            show.ScreenId = dto.ScreenId;
            show.ShowDate = dto.ShowDate; // No conversion needed now
            show.StartTime = dto.StartTime;
            show.EndTime = dto.EndTime;
            show.Price = dto.Price;

            await _showsRepository.UpdateAsync(show);

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(long id)
        {
            var show = await _showsRepository.GetByIdAsync(id);

            if (show == null)
                return Result.Failure("Show not found");

            await _showsRepository.DeleteAsync(id);

            return Result.Success();
        }
    }
}