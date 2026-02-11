using BookMyShow.Core.Entities.Core;
using System.Threading.Tasks;

namespace BookMyShow.Core.Interfaces.Core
{
    public interface IPaymentsRepository
    {
        Task<Payment?> GetByIdAsync(long id);
        Task<Payment?> GetByBookingIdAsync(long bookingId);
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task UpdateStatusAsync(long paymentId, string status);
    }
}