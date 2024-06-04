using GenericNotification.Domain.DTO;
using GenericNotification.Domain.Response;

namespace GenericNotification.Application.Interfaces;

public interface INotificationService
{
    public NotificationResponse CreateNotification(NotificationDto notificationDto);
}