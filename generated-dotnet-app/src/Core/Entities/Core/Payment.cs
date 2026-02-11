namespace BookMyShow.Core.Entities.Core
{
    public class Payment
    {
        public long PaymentId { get; set; }
        public long BookingId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public DateTime PaymentTime { get; set; } = DateTime.UtcNow;
    }
}