using Microsoft.AspNetCore.Identity;

namespace RiwiLens.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}