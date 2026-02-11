using AutoMapper;
using BookMyShow.Application.DTOs.Core.Screens;
using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Core.Common;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Application.Services.Core
{
    public class ScreensService : IScreensService
    {
        private readonly IScreensRepository _repo;
        private readonly IMapper _mapper;

        public ScreensService(IScreensRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<Screen>> GetByIdAsync(long id)
        {
            var screen = await _repo.GetByIdAsync(id);

            if (screen == null || !screen.IsActive)
                return Result<Screen>.Failure("Screen not found");

            return Result<Screen>.Success(screen);
        }

        public async Task<Result<(IEnumerable<Screen>, int)>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm)
        {
            var result = await _repo.GetPagedAsync(pageNumber, pageSize, searchTerm);
            return Result<(IEnumerable<Screen>, int)>.Success(result);
        }

        public async Task<Result<Screen>> CreateAsync(CreateScreenDto dto)
        {
            var entity = _mapper.Map<Screen>(dto);
            entity.IsActive = true;

            var created = await _repo.AddAsync(entity);
            return Result<Screen>.Success(created);
        }

        public async Task<Result> UpdateAsync(long id, UpdateScreenDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null || !existing.IsActive)
                return Result.Failure("Screen not found");

            existing.ScreenName = dto.ScreenName;
            existing.TheatreId = dto.TheatreId;
            existing.TotalSeats = dto.TotalSeats;

            await _repo.UpdateAsync(existing);
            return Result.Success();
        }

        public async Task<Result> DeactivateAsync(long id)
        {
            var screen = await _repo.GetByIdAsync(id);
            if (screen == null || !screen.IsActive)
                return Result.Failure("Screen not found");

            screen.IsActive = false;
            await _repo.UpdateAsync(screen);

            return Result.Success();
        }
    }
}
