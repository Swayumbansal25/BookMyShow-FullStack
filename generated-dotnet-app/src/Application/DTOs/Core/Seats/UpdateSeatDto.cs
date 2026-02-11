using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Application.DTOs.Core.Seats
{
    public class UpdateSeatDto
    {
        [Required]
        public long SeatId { get; set; }
        [Required]
        public long ScreenId { get; set; }
        [Required]
        public string RowLabel { get; set; } = string.Empty;
        [Required]
        public int SeatNumber { get; set; }
        public string SeatType { get; set; } = "Silver";
        public decimal BasePrice { get; set; }
    }
}