namespace BookMyShow.Core.Entities.Core
{
    public class Show
    {
        public long ShowId { get; set; }
        public long MovieId { get; set; }
        public long ScreenId { get; set; }

        public DateOnly ShowDate { get; set; } 

        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public decimal Price { get; set; }
    }
}