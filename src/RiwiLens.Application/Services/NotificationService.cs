using src.RiwiLens.Application.DTOs.Notification;
using src.RiwiLens.Application.Interfaces.Repositories;
using src.RiwiLens.Application.Interfaces.Services;
using src.RiwiLens.Domain.Entities;

namespace src.RiwiLens.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IGenericRepository<Notification> _notificationRepository;

    public NotificationService(IGenericRepository<Notification> notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<NotificationResponseDto> CreateAsync(CreateNotificationDto dto)
    {
        var notification = Notification.Create(dto.UserId, dto.Type, dto.Message);

        await _notificationRepository.AddAsync(notification);
        await _notificationRepository.SaveChangesAsync();

        return MapToDto(notification);
    }

    public async Task<IEnumerable<NotificationResponseDto>> GetMyNotificationsAsync(string userId)
    {
        var notifications = await _notificationRepository.FindAsync(n => n.UserId == userId);
        return notifications.OrderByDescending(n => n.CreatedAt).Select(MapToDto);
    }

    public async Task MarkAsReadAsync(int id, string userId)
    {
        var notification = await _notificationRepository.GetByIdAsync(id);
        if (notification == null) throw new KeyNotFoundException($"Notification {id} not found");

        if (notification.UserId != userId)
            throw new UnauthorizedAccessException("Cannot mark another user's notification as read.");

        notification.MarkAsRead();
        _notificationRepository.Update(notification);
        await _notificationRepository.SaveChangesAsync();
    }

    private static NotificationResponseDto MapToDto(Notification n)
    {
        return new NotificationResponseDto
        {
            Id = n.Id,
            UserId = n.UserId,
            Message = n.Message,
            Type = n.Type,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt
        };
    }
}
