using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using BookMyShow.Infrastructure.Data.Common;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Infrastructure.Data.Core
{
    public class ScreensRepository : IScreensRepository
    {
        private readonly string _connectionString;

        public ScreensRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Screen?> GetByIdAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Screen>(
                @"SELECT * FROM screen 
                  WHERE screen_id = @id AND is_active = true",
                new { id });
        }

        public async Task<IEnumerable<Screen>> GetAllAsync()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<Screen>(
                "SELECT * FROM screen WHERE is_active = true");
        }

       public async Task<(IEnumerable<Screen>, int)> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string? searchTerm)
{
    using var conn = new NpgsqlConnection(_connectionString);

    var filters = new Dictionary<string, object?>
    {
        { "is_active", true }
    };

    var builder = SqlQueryBuilder
        .From("screen", new[]
        {
            "screen_id",
            "screen_name",
            "theatre_id",
            "total_seats",
            "is_active"
        })
        .ApplyFilters(filters)
        .SearchAcross(new[] { "screen_name" }, searchTerm)
        .Paginate(pageNumber, pageSize)
        .OrderBy("screen_id", false);

    var (sql, param) = builder.BuildSelect();
    var (countSql, _) = builder.BuildCount();

    using var multi = await conn.QueryMultipleAsync(sql + ";" + countSql, param);

    return (
        await multi.ReadAsync<Screen>(),
        await multi.ReadSingleAsync<int>()
    );
}


        public async Task<Screen> AddAsync(Screen entity)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QuerySingleAsync<Screen>(
                @"INSERT INTO screen 
                  (screen_name, theatre_id, total_seats, is_active)
                  VALUES (@ScreenName, @TheatreId, @TotalSeats, true)
                  RETURNING *",
                entity);
        }

        public async Task UpdateAsync(Screen entity)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync(
                @"UPDATE screen SET
                  screen_name = @ScreenName,
                  theatre_id = @TheatreId,
                  total_seats = @TotalSeats,
                  is_active = @IsActive
                  WHERE screen_id = @ScreenId",
                entity);
        }
    }
}
