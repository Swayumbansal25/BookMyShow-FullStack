using BookMyShow.Application.DTOs.Core.Bookings;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Application.Interfaces.Core
{
    /// <summary>
    /// Service interface for Booking operations
    /// </summary>
    public interface IBookingsService
    {
        /// <summary>
        /// Processes a new booking, validates seat availability, and updates seat statuses
        /// </summary>
        Task<Result<Booking>> CreateBookingAsync(CreateBookingDto dto);

        /// <summary>
        /// Retrieves a booking by its unique identifier
        /// </summary>
        Task<Result<Booking>> GetByIdAsync(long id);

        /// <summary>
        /// Retrieves all bookings associated with a specific user
        /// </summary>
        Task<Result<IEnumerable<Booking>>> GetByUserIdAsync(long userId);

        /// <summary>
        /// Updates the status of an existing booking (e.g., Confirmed, Cancelled)
        /// </summary>
        Task<Result> UpdateStatusAsync(long bookingId, string status);
    }
}