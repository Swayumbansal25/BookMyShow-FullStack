using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using Dapper;
using Npgsql;

namespace BookMyShow.Infrastructure.Data.Core
{
    public class BookingsRepository : IBookingsRepository
    {
        private readonly string _connectionString;

        public BookingsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Booking> CreateBookingAsync(Booking booking, IEnumerable<BookedSeat> seats)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            using var trans = await conn.BeginTransactionAsync();

            try
            {
                var bookingSql = @"INSERT INTO bookings (user_id, show_id, total_amount, status) 
                                   VALUES (@UserId, @ShowId, @TotalAmount, @Status) RETURNING booking_id";
                
                booking.BookingId = await conn.ExecuteScalarAsync<long>(bookingSql, booking, trans);

                var seatSql = @"INSERT INTO booked_seats (booking_id, seat_id, price) 
                                VALUES (@BookingId, @SeatId, @Price)";
                
                foreach (var seat in seats)
                {
                    seat.BookingId = booking.BookingId;
                    await conn.ExecuteAsync(seatSql, seat, trans);
                }

                await trans.CommitAsync();
                return booking;
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        public async Task<Booking?> GetByIdAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Booking>("SELECT * FROM bookings WHERE booking_id = @id", new { id });
        }

        public async Task<IEnumerable<Booking>> GetByUserIdAsync(long userId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<Booking>("SELECT * FROM bookings WHERE user_id = @userId", new { userId });
        }

        public async Task UpdateStatusAsync(long bookingId, string status)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync("UPDATE bookings SET status = @status WHERE booking_id = @bookingId", new { bookingId, status });
        }
    }
}