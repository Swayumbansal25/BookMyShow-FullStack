using AutoMapper;
using BookMyShow.Application.DTOs.Core.Movies;
using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Core.Common;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Application.Services.Core
{
    public class MoviesService : IMoviesService
    {
        private readonly IMoviesRepository _repo;
        private readonly IMapper _mapper;

        public MoviesService(IMoviesRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<Result<Movies>> GetByIdAsync(long id)
        {
            var movie = await _repo.GetByIdAsync(id);

            if (movie == null)
                return Result<Movies>.Failure("Movie not found");

            return Result<Movies>.Success(movie);
        }

        public async Task<Result<IEnumerable<Movies>>> GetAllAsync()
        {
            var movies = await _repo.GetAllAsync();
            return Result<IEnumerable<Movies>>.Success(movies);
        }

        public async Task<Result<(IEnumerable<Movies>, int)>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending)
        {
            var result = await _repo.GetPagedAsync(
                pageNumber,
                pageSize,
                searchTerm,
                sortBy,
                sortDescending);

            return Result<(IEnumerable<Movies>, int)>.Success(result);
        }

        public async Task<Result<Movies>> CreateAsync(CreateMoviesDto dto)
        {
            var entity = _mapper.Map<Movies>(dto);
            var created = await _repo.AddAsync(entity);

            return Result<Movies>.Success(created);
        }

        public async Task<Result> UpdateAsync(long id, UpdateMoviesDto dto)
        {
            var entity = _mapper.Map<Movies>(dto);
            entity.MovieId = id;

            await _repo.UpdateAsync(entity);
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(long id)
        {
            await _repo.DeleteAsync(id);
            return Result.Success();
        }
    }
}
