using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Application.DTOs.Core.States
{
    public class UpdateStatesDto
    {
        [Required(ErrorMessage = "StateId is required")]
        public long StateId { get; set; }

        [Required(ErrorMessage = "StateName is required")]
        [StringLength(150, ErrorMessage = "StateName cannot exceed 150 characters")]
        public string StateName { get; set; } = string.Empty;
    }
}
