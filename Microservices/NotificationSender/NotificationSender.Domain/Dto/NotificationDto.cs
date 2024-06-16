using NotificationSender.Domain.Emun;
using NotificationSender.Domain.Entity;

namespace NotificationSender.Domain.Dto;

public class NotificationDto
{
    public Notification Notifcation { get; set; }
    public int CountNotifications { get; private set; } = -1;
    public NotificationState NotificationState { get; set; }

    public void SetNotificaitonCount(int count)
    {
        if (CountNotifications == -1)
            return;

        CountNotifications = count;
    }
}