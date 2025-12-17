using System.ComponentModel.DataAnnotations;

namespace src.RiwiLens.Application.DTOs.User;

public class UpdateUserDto
{
    [EmailAddress]
    public string? Email { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string? FullName { get; set; }
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
}
