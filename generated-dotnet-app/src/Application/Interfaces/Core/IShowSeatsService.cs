using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMyShow.Application.Interfaces.Core
{
    /// <summary>
    /// Service interface for ShowSeat operations (Availability/Status)
    /// </summary>
    public interface IShowSeatsService
    {
        /// <summary>
        /// Retrieves all seat statuses for a specific show
        /// </summary>
        Task<Result<IEnumerable<ShowSeat>>> GetByShowIdAsync(long showId);

        /// <summary>
        /// Generates show-specific seats based on a Screen's physical layout
        /// </summary>
        Task<Result> InitializeShowSeatsAsync(long showId, long screenId, decimal priceOverride = 0);

        /// <summary>
        /// Updates the status of a specific seat (e.g., from Available to Booked)
        /// </summary>
        Task<Result> UpdateStatusAsync(long id, string status);
    }
}