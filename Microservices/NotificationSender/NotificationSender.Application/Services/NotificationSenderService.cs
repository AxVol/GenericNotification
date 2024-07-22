using NotificationSender.Application.Interfaces;
using NotificationSender.Domain.Entity;

namespace NotificationSender.Application.Services;

public class NotificationSenderService : INotificationSenderService
{
    public Task SendNotificationAsync(Notification notification)
    {
        throw new NotImplementedException();
    }
}