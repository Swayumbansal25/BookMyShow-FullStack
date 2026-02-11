using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Application.DTOs.Core.Bookings
{
    public class CreateBookingDto
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public long ShowId { get; set; }
        [Required]
        public List<long> ShowSeatIds { get; set; } = new();
    }
}