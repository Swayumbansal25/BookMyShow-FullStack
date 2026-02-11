using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Application.DTOs.Core.Users;
using BookMyShow.Application.Validators.Core.Users;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using BookMyShow.Core.Common;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShow.Application.Services.Core
{
    /// <summary>
    /// Service implementation for Users operations
    /// </summary>
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;
        private readonly IMapper _mapper;

        public UsersService(IUsersRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc />
        public async Task<Result<Users>> GetByIdAsync(long id)
        {
            if (id == default(long))
                return Error.Validation("ID cannot be default value");

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return Error.NotFound($"Users with ID {id} not found");

            return Result<Users>.Success(entity);
        }

        /// <inheritdoc />
        public async Task<Result<IEnumerable<Users>>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                return Result<IEnumerable<Users>>.Success(entities);
            }
            catch (Exception ex)
            {
                return Error.Internal($"Failed to retrieve Users entities: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<Result<(IEnumerable<Users> Items, int TotalCount)>> GetPagedAsync(
            int pageNumber, 
            int pageSize, 
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDescending = false,
            Dictionary<string, object>? filters = null)
        {
            if (pageNumber < 1)
                return Error.Validation("Page number must be greater than 0");
            
            if (pageSize < 1 || pageSize > 100)
                return Error.Validation("Page size must be between 1 and 100");

            try
            {
                var result = await _repository.GetPagedAsync(
                    pageNumber, 
                    pageSize, 
                    searchTerm, 
                    sortBy, 
                    sortDescending, 
                    filters);
                    
                return Result<(IEnumerable<Users> Items, int TotalCount)>.Success(result);
            }
            catch (Exception ex)
            {
                return Error.Internal($"Failed to retrieve paginated Users entities: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<Result<Users>> CreateAsync(CreateUsersDto dto)
        {
            if (dto == null)
                return Error.Validation("DTO cannot be null");

            // Validate DTO
            var validator = new CreateUsersDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Error.Validation(errors);
            }

            // Map DTO to entity
            var entity = _mapper.Map<Users>(dto);

            try
            {
                var createdEntity = await _repository.AddAsync(entity);
                return Result<Users>.Success(createdEntity);
            }
            catch (Exception ex)
            {
                return Error.Internal($"Failed to create Users: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<Result> UpdateAsync(long id, UpdateUsersDto dto)
        {
            if (dto == null)
                return Error.Validation("DTO cannot be null");

            if (id == default(long))
                return Error.Validation("ID cannot be default value");

            // Validate DTO
            var validator = new UpdateUsersDtoValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Error.Validation(errors);
            }

            // Check if entity exists
            var existingEntity = await _repository.GetByIdAsync(id);
            if (existingEntity == null)
                return Error.NotFound($"Users with ID {id} not found");

            // Map DTO to existing entity
            _mapper.Map(dto, existingEntity);

            try
            {
                await _repository.UpdateAsync(existingEntity);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Internal($"Failed to update Users: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<Result> DeleteAsync(long id)
        {
            if (id == default(long))
                return Error.Validation("ID cannot be default value");

            var existsResult = await ExistsAsync(id);
            if (existsResult.IsFailure)
                return existsResult.Error!;

            if (!existsResult.Value)
                return Error.NotFound($"Users with ID {id} not found");

            try
            {
                await _repository.DeleteAsync(id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Error.Internal($"Failed to delete Users: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<Result<bool>> ExistsAsync(long id)
        {
            if (id == default(long))
                return Result<bool>.Success(false);

            try
            {
                var entity = await _repository.GetByIdAsync(id);
                return Result<bool>.Success(entity != null);
            }
            catch (Exception ex)
            {
                return Error.Internal($"Failed to check if Users exists: {ex.Message}");
            }
        }

    }
}