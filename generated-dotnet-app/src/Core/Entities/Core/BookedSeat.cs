namespace BookMyShow.Core.Entities.Core
{
    public class BookedSeat
    {
        public long BookingId { get; set; }
        public long SeatId { get; set; }
        public decimal Price { get; set; }
    }
}