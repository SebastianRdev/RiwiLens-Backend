using System.ComponentModel.DataAnnotations;

namespace src.RiwiLens.Application.DTOs.User;

public class UpdateUserDto
{
    [EmailAddress]
    public string? Email { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    // Add other updateable fields as necessary, e.g. Name if stored in claims or separate profile
}
