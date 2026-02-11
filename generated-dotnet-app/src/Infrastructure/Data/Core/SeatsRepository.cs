using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using BookMyShow.Infrastructure.Data.Common;
using Dapper;
using Npgsql;

namespace BookMyShow.Infrastructure.Data.Core
{
    public class SeatsRepository : ISeatsRepository
    {
        private readonly string _connectionString;

        public SeatsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Seat?> GetByIdAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Seat>("SELECT * FROM seats WHERE seat_id = @id", new { id });
        }

        public async Task<IEnumerable<Seat>> GetByScreenIdAsync(long screenId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<Seat>("SELECT * FROM seats WHERE screen_id = @screenId", new { screenId });
        }

        public async Task<(IEnumerable<Seat> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, int pageSize, string? searchTerm, string? sortBy, bool sortDescending, Dictionary<string, object>? filters)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var builder = SqlQueryBuilder.From("seats", new[] { "seat_id", "screen_id", "row_label", "seat_number", "seat_type", "base_price" })
                .SearchAcross(new[] { "row_label", "seat_type" }, searchTerm)
                .ApplyFilters(filters)
                .Paginate(pageNumber, pageSize)
                .OrderBy(sortBy ?? "seat_id", sortDescending);

            var (sql, param) = builder.BuildSelect();
            var (countSql, _) = builder.BuildCount();
            using var multi = await conn.QueryMultipleAsync(sql + ";" + countSql, param);
            return (await multi.ReadAsync<Seat>(), await multi.ReadSingleAsync<int>());
        }

        public async Task<Seat> AddAsync(Seat entity)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"INSERT INTO seats (screen_id, row_label, seat_number, seat_type, base_price) 
                        VALUES (@ScreenId, @RowLabel, @SeatNumber, @SeatType, @BasePrice) RETURNING *";
            return await conn.QuerySingleAsync<Seat>(sql, entity);
        }

        public async Task UpdateAsync(Seat entity)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"UPDATE seats SET screen_id=@ScreenId, row_label=@RowLabel, seat_number=@SeatNumber, 
                        seat_type=@SeatType, base_price=@BasePrice WHERE seat_id=@SeatId";
            await conn.ExecuteAsync(sql, entity);
        }

        public async Task DeleteAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync("DELETE FROM seats WHERE seat_id = @id", new { id });
        }
    }
}