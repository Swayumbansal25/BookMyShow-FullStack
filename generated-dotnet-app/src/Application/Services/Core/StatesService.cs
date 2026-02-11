using AutoMapper;
using BookMyShow.Application.DTOs.Core.States;
using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Application.Validators.Core.States;
using BookMyShow.Core.Common;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShow.Application.Services.Core
{
    public class StatesService : IStatesService
    {
        private readonly IStatesRepository _repository;
        private readonly IMapper _mapper;

        public StatesService(IStatesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<States>> GetByIdAsync(long id)
        {
            if (id == default)
                return Error.Validation("Invalid ID");

            var entity = await _repository.GetByIdAsync(id);
            return entity == null
                ? Error.NotFound("State not found")
                : Result<States>.Success(entity);
        }

        public async Task<Result<IEnumerable<States>>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();
            return Result<IEnumerable<States>>.Success(data);
        }

        public async Task<Result<(IEnumerable<States> Items, int TotalCount)>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDescending = false,
            Dictionary<string, object>? filters = null)
        {
            var result = await _repository.GetPagedAsync(
                pageNumber, pageSize, searchTerm, sortBy, sortDescending, filters);

            return Result<(IEnumerable<States>, int)>.Success(result);
        }

        public async Task<Result<States>> CreateAsync(CreateStatesDto dto)
        {
            var validator = new CreateStatesDtoValidator();
            var validation = validator.Validate(dto);

            if (!validation.IsValid)
                return Error.Validation(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

            var entity = _mapper.Map<States>(dto);
            var created = await _repository.AddAsync(entity);
            return Result<States>.Success(created);
        }

        public async Task<Result> UpdateAsync(long id, UpdateStatesDto dto)
        {
            if (id != dto.StateId)
                return Error.Validation("ID mismatch");

            var validator = new UpdateStatesDtoValidator();
            var validation = validator.Validate(dto);

            if (!validation.IsValid)
                return Error.Validation(string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return Error.NotFound("State not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(long id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return Error.NotFound("State not found");

            await _repository.DeleteAsync(id);
            return Result.Success();
        }
    }
}
