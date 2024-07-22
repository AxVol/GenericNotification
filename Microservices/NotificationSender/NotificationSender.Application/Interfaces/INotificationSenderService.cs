using NotificationSender.Domain.Entity;

namespace NotificationSender.Application.Interfaces;

public interface INotificationSenderService
{
    public Task SendNotificationAsync(Notification notification);
}