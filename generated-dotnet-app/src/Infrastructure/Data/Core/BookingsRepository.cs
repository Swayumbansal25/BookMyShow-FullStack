using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            // Note: booked_seats only has (booking_id, seat_id, price) - no surrogate key column.
            // We intentionally don't alias s.booking_id, since Dapper's multi-mapping splits on
            // the first column of the second type, and reusing "BookingId" for both types here
            // would create an ambiguous split point. seat_id/price come back null on the LEFT
            // JOIN if a booking has zero seats, which we filter out below.
            var sql = @"
                SELECT b.booking_id AS BookingId, b.user_id AS UserId, b.show_id AS ShowId, 
                       b.total_amount AS TotalAmount, b.booking_time AS BookingTime, b.status AS Status,
                       s.seat_id AS SeatId, s.price AS Price
                FROM bookings b
                LEFT JOIN booked_seats s ON b.booking_id = s.booking_id
                WHERE b.booking_id = @id";

            Booking? bookingEntry = null;
            var seatsCollector = new List<BookedSeat>();

            await conn.QueryAsync<Booking, BookedSeat, Booking>(
                sql,
                (booking, bookedSeat) =>
                {
                    if (bookingEntry == null)
                    {
                        bookingEntry = booking;
                    }

                    // LEFT JOIN with no matching seats still produces a BookedSeat with
                    // default values (SeatId = 0) - skip those instead of collecting them.
                    if (bookedSeat != null && bookedSeat.SeatId != 0)
                    {
                        bookedSeat.BookingId = bookingEntry.BookingId;
                        seatsCollector.Add(bookedSeat);
                    }

                    return bookingEntry;
                },
                new { id },
                splitOn: "SeatId"
            );

            if (bookingEntry != null)
            {
                bookingEntry.BookedSeats = seatsCollector;
            }

            return bookingEntry;
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