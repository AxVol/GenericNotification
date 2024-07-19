using GenericNotification.Domain.DTO;
using GenericNotification.Domain.Entity;
using GenericNotification.Domain.Response;

namespace GenericNotification.Application.Interfaces;

public interface INotificationService
{
    public Task<NotificationResponse> CreateNotificationAsync(NotificationDto notificationDto);
    public Task SendNotificationAsync(Notification notification);
    public Task<NotificationResponse> GetNotificationAsync(Guid id);
    public Task<NotificationResponse> DeleteNotificationAsync(Guid id);
}