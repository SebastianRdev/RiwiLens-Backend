using src.RiwiLens.Domain.Enums;

namespace src.RiwiLens.Application.DTOs.Coder;

public class UpdateCoderDto
{
    public string FullName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}
