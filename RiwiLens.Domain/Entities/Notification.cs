using RiwiLens.Domain.Enums;

namespace RiwiLens.Domain.Entities;

// Notifications sent to a user within the system.
public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ApplicationUser User { get; set; } = default!;
}