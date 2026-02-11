namespace BookMyShow.Core.Entities.Core
{
    public class Movies
    {
        public long MovieId { get; set; }
        public string MovieName { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public string Description { get; set; } = string.Empty;
        
        // New field for the local image path
        public string MovieImage { get; set; } = string.Empty; 
    }
}