using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using BookMyShow.Infrastructure.Data.Common;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Infrastructure.Data.Core
{
    public class CitiesRepository : ICitiesRepository
    {
        private readonly string _connectionString;

        public CitiesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Cities?> GetByIdAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Cities>(
                "SELECT * FROM city WHERE city_id = @id", new { id });
        }

        public async Task<IEnumerable<Cities>> GetAllAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<Cities>("SELECT * FROM city");
        }

        public async Task<(IEnumerable<Cities>, int)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortBy,
            bool sortDescending,
            Dictionary<string, object>? filters)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var builder = SqlQueryBuilder
                .From("city", new[] { "city_id", "state_id", "city_name" })
                .SearchAcross(new[] { "city_name" }, searchTerm)
                .ApplyFilters(filters)
                .Paginate(pageNumber, pageSize)
                .OrderBy(sortBy ?? "city_id", sortDescending);

            var (sql, param) = builder.BuildSelect();
            var (countSql, _) = builder.BuildCount();

            using var multi = await conn.QueryMultipleAsync(sql + ";" + countSql, param);
            return (await multi.ReadAsync<Cities>(), await multi.ReadSingleAsync<int>());
        }

        public async Task<Cities> AddAsync(Cities entity)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QuerySingleAsync<Cities>(
                @"INSERT INTO city (state_id, city_name)
                  VALUES (@StateId, @CityName)
                  RETURNING *", entity);
        }

        public async Task UpdateAsync(Cities entity)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync(
                @"UPDATE city SET
                  state_id = @StateId,
                  city_name = @CityName
                  WHERE city_id = @CityId", entity);
        }

        public async Task DeleteAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync(
                "DELETE FROM city WHERE city_id = @id", new { id });
        }
    }
}
