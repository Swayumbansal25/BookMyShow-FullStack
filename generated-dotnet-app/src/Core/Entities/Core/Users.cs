namespace BookMyShow.Core.Entities.Core
{
    public class Users
    {
public long UserId { get; set; }
public string FullName { get; set; } = string.Empty;
public string Email { get; set; } = string.Empty;
public string? PhoneNumber { get; set; }
public string PasswordHash { get; set; } = string.Empty;
public DateTime? DateOfBirth { get; set; }
public string? Gender { get; set; }
public bool? IsVerified { get; set; }
public bool? IsActive { get; set; }
public DateTime? CreatedAt { get; set; }
public DateTime? UpdatedAt { get; set; }
}
}