using BookMyShow.Application.Interfaces.Core;
using BookMyShow.Core.Entities.Core;
using BookMyShow.Core.Interfaces.Core;
using BookMyShow.Core.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMyShow.Application.Services.Core
{
    public class ShowSeatsService : IShowSeatsService
    {
        private readonly IShowSeatsRepository _repository;
        private readonly ISeatsRepository _seatsRepository;

        public ShowSeatsService(IShowSeatsRepository repository, ISeatsRepository seatsRepository)
        {
            _repository = repository;
            _seatsRepository = seatsRepository;
        }

        public async Task<Result<IEnumerable<ShowSeat>>> GetByShowIdAsync(long showId)
        {
            var items = await _repository.GetByShowIdAsync(showId);
            return Result<IEnumerable<ShowSeat>>.Success(items);
        }

        public async Task<Result> InitializeShowSeatsAsync(long showId, long screenId, decimal priceOverride = 0)
        {
            // 1. Get all physical seats for this screen
            var physicalSeats = await _seatsRepository.GetByScreenIdAsync(screenId);
            
            if (physicalSeats == null || !physicalSeats.Any())
            {
                return Result.Failure("No physical seats found for this screen to initialize.");
            }

            // 2. Map them to show_seats entities
            var showSeats = physicalSeats.Select(s => new ShowSeat
            {
                ShowId = showId,
                SeatId = s.SeatId,
                Status = "Available",
                Price = priceOverride > 0 ? priceOverride : s.BasePrice
            });

            // 3. Save to DB
            await _repository.CreateShowSeatsForShowAsync(showId, showSeats);
            return Result.Success();
        }

        public async Task<Result> UpdateStatusAsync(long id, string status)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return Result.Failure("ShowSeat not found");

            await _repository.UpdateStatusAsync(id, status);
            return Result.Success();
        }
    }
}