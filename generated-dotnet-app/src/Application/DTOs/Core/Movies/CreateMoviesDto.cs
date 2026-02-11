using System;
using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Application.DTOs.Core.Movies
{
    public class CreateMoviesDto
    {
        [Required]
        [StringLength(200)]
        public string MovieName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Language { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Genre { get; set; } = string.Empty;

        [Range(1, 500)]
        public int DurationMinutes { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
