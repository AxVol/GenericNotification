using GenericNotification.Domain.DTO;
using GenericNotification.Domain.Response;

namespace GenericNotification.Application.Interfaces;

public interface INotificationService
{
    public Task<NotificationResponse> CreateNotificationAsync(NotificationDto notificationDto);
}