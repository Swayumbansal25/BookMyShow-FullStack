using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Application.DTOs.Core.Theatres
{
    public class UpdateTheatresDto
    {
        [Required]
        public long TheatreId { get; set; }

        [Required]
        public long CityId { get; set; }

        [Required]
        [StringLength(200)]
        public string TheatreName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;
    }
}
