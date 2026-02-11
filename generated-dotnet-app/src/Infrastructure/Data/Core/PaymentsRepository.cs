using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using Dapper;
using Npgsql;

namespace BookMyShow.Infrastructure.Data.Core
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly string _connectionString;

        public PaymentsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"INSERT INTO payments (booking_id, payment_method, transaction_id, amount, payment_status, payment_time) 
                        VALUES (@BookingId, @PaymentMethod, @TransactionId, @Amount, @PaymentStatus, @PaymentTime) 
                        RETURNING payment_id";
            
            payment.PaymentId = await conn.ExecuteScalarAsync<long>(sql, payment);
            return payment;
        }

        public async Task<Payment?> GetByIdAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Payment>("SELECT * FROM payments WHERE payment_id = @id", new { id });
        }

        public async Task<Payment?> GetByBookingIdAsync(long bookingId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Payment>("SELECT * FROM payments WHERE booking_id = @bookingId", new { bookingId });
        }

        public async Task UpdateStatusAsync(long paymentId, string status)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.ExecuteAsync("UPDATE payments SET payment_status = @status WHERE payment_id = @paymentId", new { paymentId, status });
        }
    }
}