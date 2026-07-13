using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Application.DTOs.Core.Payments;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using BookMyShow.Core.Common;

namespace BookMyShow.Application.Services.Core
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IPaymentsRepository _repository;
        private readonly IBookingsRepository _bookingsRepository;

        public PaymentsService(
            IPaymentsRepository repository,
            IBookingsRepository bookingsRepository)
        {
            _repository = repository;
            _bookingsRepository = bookingsRepository;
        }

        public async Task<Result<Payment>> ProcessPaymentAsync(ProcessPaymentDto dto)
        {
            try
            {
                // 1. Verify Booking exists
                var booking = await _bookingsRepository.GetByIdAsync(dto.BookingId);
                if (booking == null)
                    return Result<Payment>.Failure("Booking not found");

                // 2. Guard against a missing/zero TotalAmount causing a downstream
                //    constraint failure when saving the Payment row.
                if (booking.TotalAmount <= 0)
                    return Result<Payment>.Failure(
                        $"Booking {dto.BookingId} has an invalid total amount ({booking.TotalAmount}).");

                // 3. Create Payment Entity
                var payment = new Payment
                {
                    BookingId = dto.BookingId,
                    PaymentMethod = dto.PaymentMethod,
                    TransactionId = dto.TransactionId,
                    Amount = booking.TotalAmount,
                    PaymentStatus = "Success", // In a real app, this would come from a Payment Gateway
                    PaymentTime = DateTime.UtcNow
                };

                // 4. Save Payment
                var createdPayment = await _repository.CreatePaymentAsync(payment);

                // 5. Update Booking Status to Confirmed
                await _bookingsRepository.UpdateStatusAsync(dto.BookingId, "Confirmed");

                return Result<Payment>.Success(createdPayment);
            }
            catch (Exception ex)
            {
                // Catch here so the controller can return a clean 400 (with CORS headers
                // intact) instead of letting the exception bubble up as a raw 500.
                return Result<Payment>.Failure($"Payment processing failed: {ex.Message}");
            }
        }

        public async Task<Result<Payment>> GetByBookingIdAsync(long bookingId)
        {
            try
            {
                var payment = await _repository.GetByBookingIdAsync(bookingId);
                return payment == null ? Result<Payment>.Failure("Payment not found") : Result<Payment>.Success(payment);
            }
            catch (Exception ex)
            {
                return Result<Payment>.Failure($"Failed to fetch payment: {ex.Message}");
            }
        }
    }
}