namespace BookMyShow.Core.Entities.Core
{
    public class Theatres
    {
        public long TheatreId { get; set; }
        public long CityId { get; set; }
        public string TheatreName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
