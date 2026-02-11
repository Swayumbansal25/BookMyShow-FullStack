using BookMyShow.Application.DTOs.Core.Payments;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Common;
using System.Threading.Tasks;

namespace BookMyShow.Application.Interfaces.Core
{
    /// <summary>
    /// Service interface for Payment operations
    /// </summary>
    public interface IPaymentsService
    {
        /// <summary>
        /// Processes a payment transaction and updates the associated booking status
        /// </summary>
        Task<Result<Payment>> ProcessPaymentAsync(ProcessPaymentDto dto);

        /// <summary>
        /// Retrieves payment details associated with a specific booking
        /// </summary>
        Task<Result<Payment>> GetByBookingIdAsync(long bookingId);
    }
}