using BookMyShow.Core.Entities.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Core.Interfaces.Core
{
    public interface IBookingsRepository
    {
        Task<Booking?> GetByIdAsync(long id);
        Task<IEnumerable<Booking>> GetByUserIdAsync(long userId);
        Task<Booking> CreateBookingAsync(Booking booking, IEnumerable<BookedSeat> seats);
        Task UpdateStatusAsync(long bookingId, string status);
    }
}