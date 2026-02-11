namespace BookMyShow.Core.Entities.Core
{
    public class ShowSeat
    {
        public long ShowSeatId { get; set; }
        public long ShowId { get; set; }
        public long SeatId { get; set; }
        public string Status { get; set; } = "Available";
        public decimal Price { get; set; }
    }
}