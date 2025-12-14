using src.RiwiLens.Domain.Enums;

namespace src.RiwiLens.Domain.Entities;

// Notifications sent to a user within the system.
public class Notification
{
    public int Id { get; private set; }
    public string UserId { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public NotificationType Type { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }


    private Notification() { }

    private Notification(string userId, string message, NotificationType type)
    {
        UserId = userId;
        Message = message;
        Type = type;
        IsRead = false;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Notification Create(
        string userId,
        NotificationType type,
        string message)
    {
        if (type == NotificationType.Unknown)
            throw new InvalidOperationException("Invalid notification type.");
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("Invalid UserId.");
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Required message.");

        return new Notification
        {
            UserId = userId,
            Type = type,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkAsRead()
    {
        if (IsRead) return;

        IsRead = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
