using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Application.DTOs.Core.Screens
{
    public class UpdateScreenDto
    {
        [Required]
        [StringLength(100)]
        public string ScreenName { get; set; } = string.Empty;

        [Required]
        public long TheatreId { get; set; }

        [Range(1, 1000)]
        public int TotalSeats { get; set; }
    }
}
