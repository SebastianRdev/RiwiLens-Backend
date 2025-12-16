namespace src.RiwiLens.Application.DTOs.Dashboard;

public class UserResponseDto
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Clan { get; set; } = "N/A";
    public string Status { get; set; } = "Active"; // Default to Active for Admins/others
}
