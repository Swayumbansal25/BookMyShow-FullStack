using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Application.DTOs.Core.Cities
{
    public class CreateCitiesDto
    {
        [Required]
        public long StateId { get; set; }

        [Required]
        [StringLength(150)]
        public string CityName { get; set; } = string.Empty;
    }
}
