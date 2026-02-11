using AutoMapper;
using BookMyShow.Application.DTOs.Core.Theatres;
using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Application.Validators.Core.Theatres;
using BookMyShow.Core.Common;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShow.Application.Services.Core
{
    public class TheatresService : ITheatresService
    {
        private readonly ITheatresRepository _repo;
        private readonly IMapper _mapper;

        public TheatresService(ITheatresRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<Theatres>> GetByIdAsync(long id)
        {
            var data = await _repo.GetByIdAsync(id);
            return data == null ? Error.NotFound("Theatre not found") : Result<Theatres>.Success(data);
        }

        public async Task<Result<IEnumerable<Theatres>>> GetAllAsync()
            => Result<IEnumerable<Theatres>>.Success(await _repo.GetAllAsync());

        public async Task<Result<(IEnumerable<Theatres>, int)>> GetPagedAsync(
            int pageNumber, int pageSize, string? searchTerm,
            string? sortBy, bool sortDescending, Dictionary<string, object>? filters)
            => Result<(IEnumerable<Theatres>, int)>.Success(
                await _repo.GetPagedAsync(pageNumber, pageSize, searchTerm, sortBy, sortDescending, filters));

        public async Task<Result<Theatres>> CreateAsync(CreateTheatresDto dto)
        {
            var val = new CreateTheatresDtoValidator().Validate(dto);
            if (!val.IsValid)
                return Error.Validation(string.Join("; ", val.Errors.Select(e => e.ErrorMessage)));

            return Result<Theatres>.Success(await _repo.AddAsync(_mapper.Map<Theatres>(dto)));
        }

        public async Task<Result> UpdateAsync(long id, UpdateTheatresDto dto)
        {
            if (id != dto.TheatreId) return Error.Validation("ID mismatch");

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return Error.NotFound("Theatre not found");

            _mapper.Map(dto, existing);
            await _repo.UpdateAsync(existing);
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(long id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return Error.NotFound("Theatre not found");

            await _repo.DeleteAsync(id);
            return Result.Success();
        }
    }
}
