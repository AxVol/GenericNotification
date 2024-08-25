using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NotificationSender.DAL.Repository.Interfaces;
using NotificationSender.Domain.Emun;
using NotificationSender.Domain.Entity;

namespace NotificationSender.Tests.Mock;

public static class MockFactory<T> where T : class
{
    public static INotificationRepository GetNotificationRepository()
    {
        Notification notification = GetNotification();
        var mock = new Mock<INotificationRepository>();

        mock.Setup(m => m.AddAsync(It.IsAny<Notification>())).Returns(Task.CompletedTask);
        mock.Setup(m => m.DeleteAsync(It.IsAny<Notification>())).Returns(Task.CompletedTask);
        mock.Setup(m => m.UpdateAsync(It.IsAny<Notification>())).Returns(Task.CompletedTask);
        mock.Setup(m => m.GetAsync(It.IsAny<Notification>())).Returns(Task.FromResult(notification));

        return mock.Object;
    }
    
    public static ILogger<T> GetLogger()
    {
        var mock = new Mock<ILogger<T>>();

        return mock.Object;
    }

    private static Notification GetNotification()
    {
        string uuid = "4dfc6b14-7213-3363-8009-b23c56e3a1b1";
        Guid guid;
        bool isGuid = Guid.TryParse(uuid, out guid);
        string uuidStatus = "4dfc6b14-7213-2002-8009-b23c56e3a1b1";
        Guid notificationStatusGuid;
        bool isGuidStatus = Guid.TryParse(uuidStatus, out notificationStatusGuid);

        Notification notification = new Notification()
        {
            Id = guid,
            Title = "test",
            Body = "test",
            TimeToSend = DateTime.Now.ToUniversalTime(),
            ForUsers = new List<NotificationStatus>()
            {
                new NotificationStatus()
                {
                    Id = notificationStatusGuid,
                    Email = "test@mail.ru",
                    SendStatus = false
                }
            },
            CreatorName = "test",
            NotificationState = NotificationState.NotStarted,
        };
        notification.CountNotifications = 3;
        
        return notification;
    }
}