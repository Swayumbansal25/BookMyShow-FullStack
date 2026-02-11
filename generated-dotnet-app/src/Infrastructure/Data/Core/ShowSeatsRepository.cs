using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using Dapper;
using Npgsql;

namespace BookMyShow.Infrastructure.Data.Core
{
    public class ShowSeatsRepository : IShowSeatsRepository
    {
        private readonly string _connectionString;

        public ShowSeatsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ShowSeat?> GetByIdAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<ShowSeat>(
                "SELECT * FROM show_seats WHERE show_seat_id = @id", new { id });
        }

        public async Task<IEnumerable<ShowSeat>> GetByShowIdAsync(long showId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<ShowSeat>(
                "SELECT * FROM show_seats WHERE show_id = @showId", new { showId });
        }

        public async Task<int> CreateShowSeatsForShowAsync(long showId, IEnumerable<ShowSeat> showSeats)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"INSERT INTO show_seats (show_id, seat_id, status, price) 
                        VALUES (@ShowId, @SeatId, @Status, @Price)";
            return await conn.ExecuteAsync(sql, showSeats);
        }

        public async Task UpdateStatusAsync(long id, string status)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync(
                "UPDATE show_seats SET status = @status WHERE show_seat_id = @id", 
                new { id, status });
        }

        public async Task DeleteByShowIdAsync(long showId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync("DELETE FROM show_seats WHERE show_id = @showId", new { showId });
        }
    }
}