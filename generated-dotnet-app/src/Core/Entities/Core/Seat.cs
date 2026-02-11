namespace BookMyShow.Core.Entities.Core
{
    public class Seat
    {
        public long SeatId { get; set; }
        public long ScreenId { get; set; }
        public string RowLabel { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
        public string SeatType { get; set; } = "Silver";
        public decimal BasePrice { get; set; }
    }
}