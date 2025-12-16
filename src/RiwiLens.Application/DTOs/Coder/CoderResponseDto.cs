using src.RiwiLens.Domain.Enums;

namespace src.RiwiLens.Application.DTOs.Coder;

public class CoderResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty; // From ApplicationUser
    public DocumentType DocumentType { get; set; }
    public string Identification { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public int StatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public int ProfessionalProfileId { get; set; }
}
