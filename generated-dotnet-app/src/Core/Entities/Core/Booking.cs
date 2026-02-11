namespace BookMyShow.Core.Entities.Core
{
    public class Booking
    {
        public long BookingId { get; set; }
        public long UserId { get; set; }
        public long ShowId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime BookingTime { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled
    }
}