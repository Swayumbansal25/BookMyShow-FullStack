using System.ComponentModel.DataAnnotations;

namespace BookMyShow.Application.DTOs.Core.Users
{
    /// <summary>
    /// Data transfer object for creating Users
    /// </summary>
    public class CreateUsersDto
    {
/// <summary>
        /// FullName
        /// </summary>
        [Required(ErrorMessage = "FullName is required")]
        [StringLength(255, ErrorMessage = "FullName cannot exceed 255 characters")]
        public string FullName { get; set; } = string.Empty;
/// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string Email { get; set; } = string.Empty;
/// <summary>
        /// PhoneNumber
        /// </summary>
        [StringLength(255, ErrorMessage = "PhoneNumber cannot exceed 255 characters")]
        public string? PhoneNumber { get; set; }
/// <summary>
        /// PasswordHash
        /// </summary>
        [Required(ErrorMessage = "PasswordHash is required")]
        [StringLength(255, ErrorMessage = "PasswordHash cannot exceed 255 characters")]
        public string PasswordHash { get; set; } = string.Empty;
/// <summary>
        /// DateOfBirth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
/// <summary>
        /// Gender
        /// </summary>
        [StringLength(255, ErrorMessage = "Gender cannot exceed 255 characters")]
        public string? Gender { get; set; }
/// <summary>
        /// IsVerified
        /// </summary>
        public bool? IsVerified { get; set; }
/// <summary>
        /// IsActive
        /// </summary>
        public bool? IsActive { get; set; }
/// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime? CreatedAt { get; set; }
/// <summary>
        /// UpdatedAt
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
}
}