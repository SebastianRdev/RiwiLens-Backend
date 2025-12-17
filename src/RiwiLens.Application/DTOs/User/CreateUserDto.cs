using src.RiwiLens.Domain.Enums;

namespace src.RiwiLens.Application.DTOs.User;

public class CreateUserDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // Admin, TeamLeader, Coder

    // Common Profile Data
    public Gender Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public string Identification { get; set; } = string.Empty;
    public DocumentType DocumentType { get; set; }

    // Coder Specific (Optional)
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }

    // Clan Assignment
    public int? ClanId { get; set; }
}
