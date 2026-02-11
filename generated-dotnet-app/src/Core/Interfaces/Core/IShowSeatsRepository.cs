using BookMyShow.Core.Entities.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Core.Interfaces.Core
{
    public interface IShowSeatsRepository
    {
        Task<ShowSeat?> GetByIdAsync(long id);
        Task<IEnumerable<ShowSeat>> GetByShowIdAsync(long showId);
        Task<int> CreateShowSeatsForShowAsync(long showId, IEnumerable<ShowSeat> showSeats);
        Task UpdateStatusAsync(long showSeatId, string status);
        Task DeleteByShowIdAsync(long showId);
    }
}