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

        public PaymentsService(IPaymentsRepository repository, IBookingsRepository bookingsRepository)
        {
            _repository = repository;
            _bookingsRepository = bookingsRepository;
        }

        public async Task<Result<Payment>> ProcessPaymentAsync(ProcessPaymentDto dto)
        {
            // 1. Verify Booking exists
            var booking = await _bookingsRepository.GetByIdAsync(dto.BookingId);
            if (booking == null) return Result<Payment>.Failure("Booking not found");

            // 2. Create Payment Entity
            var payment = new Payment
            {
                BookingId = dto.BookingId,
                PaymentMethod = dto.PaymentMethod,
                TransactionId = dto.TransactionId,
                Amount = booking.TotalAmount,
                PaymentStatus = "Success", // In a real app, this would come from a Payment Gateway
                PaymentTime = DateTime.UtcNow
            };

            // 3. Save Payment
            var createdPayment = await _repository.CreatePaymentAsync(payment);

            // 4. Update Booking Status to Confirmed
            await _bookingsRepository.UpdateStatusAsync(dto.BookingId, "Confirmed");

            return Result<Payment>.Success(createdPayment);
        }

        public async Task<Result<Payment>> GetByBookingIdAsync(long bookingId)
        {
            var payment = await _repository.GetByBookingIdAsync(bookingId);
            return payment == null ? Result<Payment>.Failure("Payment not found") : Result<Payment>.Success(payment);
        }
    }
}