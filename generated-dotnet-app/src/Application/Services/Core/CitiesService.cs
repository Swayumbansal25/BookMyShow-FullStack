using AutoMapper;
using BookMyShow.Application.DTOs.Core.Cities;
using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Application.Validators.Core.Cities;
using BookMyShow.Core.Common;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShow.Application.Services.Core
{
    public class CitiesService : ICitiesService
    {
        private readonly ICitiesRepository _repo;
        private readonly IMapper _mapper;

        public CitiesService(ICitiesRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<Cities>> GetByIdAsync(long id)
        {
            var data = await _repo.GetByIdAsync(id);
            return data == null ? Error.NotFound("City not found") : Result<Cities>.Success(data);
        }

        public async Task<Result<IEnumerable<Cities>>> GetAllAsync()
            => Result<IEnumerable<Cities>>.Success(await _repo.GetAllAsync());

        public async Task<Result<(IEnumerable<Cities>, int)>> GetPagedAsync(
            int pageNumber, int pageSize, string? searchTerm,
            string? sortBy, bool sortDescending, Dictionary<string, object>? filters)
            => Result<(IEnumerable<Cities>, int)>.Success(
                await _repo.GetPagedAsync(pageNumber, pageSize, searchTerm, sortBy, sortDescending, filters));

        public async Task<Result<Cities>> CreateAsync(CreateCitiesDto dto)
        {
            var val = new CreateCitiesDtoValidator().Validate(dto);
            if (!val.IsValid)
                return Error.Validation(string.Join("; ", val.Errors.Select(e => e.ErrorMessage)));

            return Result<Cities>.Success(await _repo.AddAsync(_mapper.Map<Cities>(dto)));
        }

        public async Task<Result> UpdateAsync(long id, UpdateCitiesDto dto)
        {
            if (id != dto.CityId) return Error.Validation("ID mismatch");

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return Error.NotFound("City not found");

            _mapper.Map(dto, existing);
            await _repo.UpdateAsync(existing);
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(long id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return Error.NotFound("City not found");

            await _repo.DeleteAsync(id);
            return Result.Success();
        }
    }
}
