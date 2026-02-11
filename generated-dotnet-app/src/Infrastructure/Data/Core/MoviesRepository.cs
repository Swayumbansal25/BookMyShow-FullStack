using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using BookMyShow.Infrastructure.Data.Common;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Infrastructure.Data.Core
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly string _connectionString;

        public MoviesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Movies?> GetByIdAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Movies>(
                "SELECT * FROM movie WHERE movie_id = @id", new { id });
        }

        public async Task<IEnumerable<Movies>> GetAllAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<Movies>("SELECT * FROM movie");
        }

        public async Task<(IEnumerable<Movies>, int)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var builder = SqlQueryBuilder
                .From("movie", new[] {
                    "movie_id", "movie_name", "language",
                    "genre", "duration_minutes",
                    "release_date", "description"
                })
                .SearchAcross(new[] { "movie_name", "language", "genre" }, searchTerm)
                .Paginate(pageNumber, pageSize)
                .OrderBy(sortBy ?? "movie_id", sortDescending);

            var (sql, param) = builder.BuildSelect();
            var (countSql, _) = builder.BuildCount();

            using var multi = await conn.QueryMultipleAsync(sql + ";" + countSql, param);
            return (await multi.ReadAsync<Movies>(), await multi.ReadSingleAsync<int>());
        }

        public async Task<Movies> AddAsync(Movies entity)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QuerySingleAsync<Movies>(
                @"INSERT INTO movie
                  (movie_name, language, genre, duration_minutes, release_date, description)
                  VALUES (@MovieName, @Language, @Genre, @DurationMinutes, @ReleaseDate, @Description)
                  RETURNING *", entity);
        }

        public async Task UpdateAsync(Movies entity)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync(
                @"UPDATE movie SET
                  movie_name = @MovieName,
                  language = @Language,
                  genre = @Genre,
                  duration_minutes = @DurationMinutes,
                  release_date = @ReleaseDate,
                  description = @Description
                  WHERE movie_id = @MovieId", entity);
        }

        public async Task DeleteAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync(
                "DELETE FROM movie WHERE movie_id = @id", new { id });
        }
    }
}
