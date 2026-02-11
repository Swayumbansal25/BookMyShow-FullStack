using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Application.DTOs.Core.Seats;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using BookMyShow.Core.Common;
using AutoMapper;

namespace BookMyShow.Application.Services.Core
{
    public class SeatsService : ISeatsService
    {
        private readonly ISeatsRepository _repository;
        private readonly IMapper _mapper;

        public SeatsService(ISeatsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<Seat>> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? Result<Seat>.Failure("Seat not found") : Result<Seat>.Success(entity);
        }

        public async Task<Result<IEnumerable<Seat>>> GetByScreenIdAsync(long screenId)
        {
            var entities = await _repository.GetByScreenIdAsync(screenId);
            return Result<IEnumerable<Seat>>.Success(entities);
        }

        public async Task<Result<(IEnumerable<Seat> Items, int TotalCount)>> GetPagedAsync(
            int pageNumber, int pageSize, string? searchTerm, string? sortBy, bool sortDescending, Dictionary<string, object>? filters)
        {
            var result = await _repository.GetPagedAsync(pageNumber, pageSize, searchTerm, sortBy, sortDescending, filters);
            return Result<(IEnumerable<Seat>, int)>.Success(result);
        }

        public async Task<Result<Seat>> CreateAsync(CreateSeatDto dto)
        {
            var entity = _mapper.Map<Seat>(dto);
            var created = await _repository.AddAsync(entity);
            return Result<Seat>.Success(created);
        }

        public async Task<Result> UpdateAsync(long id, UpdateSeatDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return Result.Failure("Seat not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(long id)
        {
            await _repository.DeleteAsync(id);
            return Result.Success();
        }
    }
}