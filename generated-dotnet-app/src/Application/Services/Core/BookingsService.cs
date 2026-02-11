using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Application.DTOs.Core.Bookings;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using BookMyShow.Core.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Application.Services.Core
{
    public class BookingsService : IBookingsService
    {
        private readonly IBookingsRepository _repository;
        private readonly IShowSeatsRepository _showSeatsRepository;

        public BookingsService(IBookingsRepository repository, IShowSeatsRepository showSeatsRepository)
        {
            _repository = repository;
            _showSeatsRepository = showSeatsRepository;
        }

        public async Task<Result<Booking>> CreateBookingAsync(CreateBookingDto dto)
        {
            decimal totalAmount = 0;
            var bookedSeats = new List<BookedSeat>();

            // 1. Validate and Calculate Total
            foreach (var showSeatId in dto.ShowSeatIds)
            {
                var showSeat = await _showSeatsRepository.GetByIdAsync(showSeatId);
                if (showSeat == null || showSeat.Status != "Available")
                    return Result<Booking>.Failure($"Seat with ID {showSeatId} is not available.");

                totalAmount += showSeat.Price;
                bookedSeats.Add(new BookedSeat 
                { 
                    SeatId = showSeat.SeatId, 
                    Price = showSeat.Price 
                });
            }

            // 2. Create Entity
            var booking = new Booking
            {
                UserId = dto.UserId,
                ShowId = dto.ShowId,
                TotalAmount = totalAmount,
                BookingTime = DateTime.UtcNow,
                Status = "Pending"
            };

            try
            {
                // 3. Save Booking (Transaction handled in Repository)
                var createdBooking = await _repository.CreateBookingAsync(booking, bookedSeats);

                // 4. Update ShowSeat Statuses
                foreach (var showSeatId in dto.ShowSeatIds)
                {
                    await _showSeatsRepository.UpdateStatusAsync(showSeatId, "Booked");
                }

                return Result<Booking>.Success(createdBooking);
            }
            catch (Exception ex)
            {
                return Result<Booking>.Failure($"Booking failed: {ex.Message}");
            }
        }

        // Implementation for IBookingsService.GetByIdAsync
        public async Task<Result<Booking>> GetByIdAsync(long id)
        {
            var booking = await _repository.GetByIdAsync(id);
            if (booking == null)
                return Result<Booking>.Failure("Booking not found");

            return Result<Booking>.Success(booking);
        }

        // Implementation for IBookingsService.GetByUserIdAsync
        public async Task<Result<IEnumerable<Booking>>> GetByUserIdAsync(long userId)
        {
            var bookings = await _repository.GetByUserIdAsync(userId);
            return Result<IEnumerable<Booking>>.Success(bookings);
        }

        // Implementation for IBookingsService.UpdateStatusAsync
        public async Task<Result> UpdateStatusAsync(long bookingId, string status)
        {
            var booking = await _repository.GetByIdAsync(bookingId);
            if (booking == null)
                return Result.Failure("Booking not found");

            await _repository.UpdateStatusAsync(bookingId, status);
            return Result.Success();
        }
    }
}