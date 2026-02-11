using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using BookMyShow.Infrastructure.Data.Common;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Infrastructure.Data.Core
{
    public class TheatresRepository : ITheatresRepository
    {
        private readonly string _connectionString;

        public TheatresRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Theatres?> GetByIdAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Theatres>(
                "SELECT * FROM theatre WHERE theatre_id = @id", new { id });
        }

        public async Task<IEnumerable<Theatres>> GetAllAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<Theatres>("SELECT * FROM theatre");
        }

        public async Task<(IEnumerable<Theatres>, int)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending,
            Dictionary<string, object>? filters)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var builder = SqlQueryBuilder
                .From("theatre", new[] { "theatre_id", "city_id", "theatre_name", "address" })
                .SearchAcross(new[] { "theatre_name", "address" }, searchTerm)
                .ApplyFilters(filters)
                .Paginate(pageNumber, pageSize)
                .OrderBy(sortBy ?? "theatre_id", sortDescending);

            var (sql, param) = builder.BuildSelect();
            var (countSql, _) = builder.BuildCount();

            using var multi = await conn.QueryMultipleAsync(sql + ";" + countSql, param);
            return (await multi.ReadAsync<Theatres>(), await multi.ReadSingleAsync<int>());
        }

        public async Task<Theatres> AddAsync(Theatres entity)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QuerySingleAsync<Theatres>(
                @"INSERT INTO theatre (city_id, theatre_name, address)
                  VALUES (@CityId, @TheatreName, @Address)
                  RETURNING *", entity);
        }

        public async Task UpdateAsync(Theatres entity)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync(
                @"UPDATE theatre SET
                  city_id = @CityId,
                  theatre_name = @TheatreName,
                  address = @Address
                  WHERE theatre_id = @TheatreId", entity);
        }

        public async Task DeleteAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync(
                "DELETE FROM theatre WHERE theatre_id = @id", new { id });
        }
    }
}
