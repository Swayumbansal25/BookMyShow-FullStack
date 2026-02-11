using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Application.DTOs.Core.States
{
    public class CreateStatesDto
    {
        [Required(ErrorMessage = "StateName is required")]
        [StringLength(150, ErrorMessage = "StateName cannot exceed 150 characters")]
        public string StateName { get; set; } = string.Empty;
    }
}
