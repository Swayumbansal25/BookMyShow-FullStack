using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Application.DTOs.Core.Cities
{
    public class UpdateCitiesDto
    {
        [Required]
        public long CityId { get; set; }

        [Required]
        public long StateId { get; set; }

        [Required]
        [StringLength(150)]
        public string CityName { get; set; } = string.Empty;
    }
}
