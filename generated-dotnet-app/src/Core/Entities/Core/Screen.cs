namespace BookMyShow.Core.Entities.Core
{
    public class Screen
    {
        public long ScreenId { get; set; }
        public string ScreenName { get; set; } = string.Empty;
        public long TheatreId { get; set; }
        public int TotalSeats { get; set; }

        // ✅ Soft delete flag
        public bool IsActive { get; set; } = true;
    }
}
