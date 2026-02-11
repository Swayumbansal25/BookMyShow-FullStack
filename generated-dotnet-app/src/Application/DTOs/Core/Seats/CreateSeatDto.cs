using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Application.DTOs.Core.Seats
{
    public class CreateSeatDto
    {
        [Required(ErrorMessage = "ScreenId is required")]
        public long ScreenId { get; set; }

        [Required(ErrorMessage = "RowLabel is required")]
        [StringLength(10)]
        public string RowLabel { get; set; } = string.Empty;

        [Required(ErrorMessage = "SeatNumber is required")]
        public int SeatNumber { get; set; }

        [StringLength(50)]
        public string SeatType { get; set; } = "Silver";

        [Range(0, 10000)]
        public decimal BasePrice { get; set; }
    }
}