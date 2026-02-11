using System;
using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Application.DTOs.Core.Shows
{
    public class UpdateShowDto
    {
        [Required]
        public long MovieId { get; set; }

        [Required]
        public long ScreenId { get; set; }

        [Required]
        public DateOnly ShowDate { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Range(0, 10000)]
        public decimal Price { get; set; }
    }
}
