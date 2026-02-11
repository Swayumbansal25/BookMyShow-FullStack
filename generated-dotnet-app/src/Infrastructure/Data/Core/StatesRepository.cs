using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using BookMyShow.Infrastructure.Data.Common;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Infrastructure.Data.Core
{
    public class StatesRepository : IStatesRepository
    {
        private readonly string _connectionString;

        public StatesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<States?> GetByIdAsync(long id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<States>(
                "SELECT * FROM states WHERE state_id = @id",
                new { id });
        }

        public async Task<IEnumerable<States>> GetAllAsync()
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QueryAsync<States>("SELECT * FROM states");
        }

        public async Task<(IEnumerable<States> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm = null,
            string? sortBy = null,
            bool sortDescending = false,
            Dictionary<string, object>? filters = null)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var allowedColumns = new[] { "state_id", "state_name" };

            var builder = SqlQueryBuilder
                .From("states", allowedColumns)
                .SearchAcross(new[] { "state_name" }, searchTerm)
                .ApplyFilters(filters)
                .Paginate(pageNumber, pageSize);

            builder.OrderBy(
                string.IsNullOrWhiteSpace(sortBy) ? "state_id" : sortBy!,
                sortDescending);

            var (dataSql, parameters) = builder.BuildSelect();
            var (countSql, _) = builder.BuildCount();

            using var multi = await connection.QueryMultipleAsync(dataSql + ";" + countSql, parameters);
            var items = await multi.ReadAsync<States>();
            var totalCount = await multi.ReadSingleAsync<int>();

            return (items, totalCount);
        }

        public async Task<States> AddAsync(States entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            return await connection.QuerySingleAsync<States>(
                @"INSERT INTO states (state_name)
                  VALUES (@StateName)
                  RETURNING *", entity);
        }

        public async Task UpdateAsync(States entity)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                @"UPDATE states
                  SET state_name = @StateName
                  WHERE state_id = @StateId", entity);
        }

        public async Task DeleteAsync(long id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "DELETE FROM states WHERE state_id = @id",
                new { id });
        }
    }
}
