using src.RiwiLens.Application.DTOs.Notification;

namespace src.RiwiLens.Application.Interfaces.Services;

public interface INotificationService
{
    Task<NotificationResponseDto> CreateAsync(CreateNotificationDto dto);
    Task<IEnumerable<NotificationResponseDto>> GetMyNotificationsAsync(string userId);
    Task MarkAsReadAsync(int id, string userId);
}
