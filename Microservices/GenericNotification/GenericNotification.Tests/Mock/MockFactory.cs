using GenericNotification.Application.Interfaces;
using GenericNotification.Application.Service;
using GenericNotification.DAL.Repository.Interfaces;
using GenericNotification.Domain.Entity;
using GenericNotification.Domain.Resources;
using GenericNotification.Producer.Interfaces;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace GenericNotification.Tests.Mock;

public class MockFactory<T> where T : class
{
    private static readonly IStringLocalizer<Resources>? instance = null;

    public static ILogger<T> GetLogger()
    {
        var mock = new Mock<ILogger<T>>();

        return mock.Object;
    }

    public static IRepository<Notification> GetNotificationRepository()
    {
        IEnumerable<Notification> repositories = GetNotificationList();
        var mock = new Mock<IRepository<Notification>>();

        mock.Setup(m => m.CreateAsync(It.IsAny<Notification>())).Returns(Task.CompletedTask);
        mock.Setup(m => m.DeleteAsync(It.IsAny<Notification>())).Returns(Task.CompletedTask);
        mock.Setup(m => m.UpdateAsync(It.IsAny<Notification>())).Returns(Task.CompletedTask);
        mock.Setup(m => m.GetAll()).Returns(repositories);

        return mock.Object;
    }

    public static IParser GetParser()
    {
        IParser parser = new Parser(GetLocalaizer());

        return parser;
    }

    public static IStringLocalizer<Resources> GetLocalaizer()
    {
        if (instance == null)
        {
            var options = Options.Create(new LocalizationOptions() { ResourcesPath = "../../Core/Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            var localaizer = new StringLocalizer<Resources>(factory);
            
            return localaizer;
        }
        else
        {
            return instance;
        }
    }

    public static IProducer GetRabbitMq()
    {
        var mock = new Mock<IProducer>();
        mock.Setup(m => m.Publish("message", "routingKey")).Returns(Task.CompletedTask);

        return mock.Object;
    }

    private static IEnumerable<Notification> GetNotificationList()
    {
        List<Notification> notifications = new List<Notification>();
        string uuid = "4dfc6b14-7213-3363-8009-b23c56e3a1b1";
        Guid guid;
        bool isGuid = Guid.TryParse(uuid, out guid);

        Notification notification = new Notification()
        {
            Id = guid,
            Title = "test",
            Body = "test",
            TimeToSend = DateTime.Now.ToUniversalTime(),
            IsSend = false,
            ForUsers = new List<NotificationStatus>(),
            CreatorName = "test"
        };

        notifications.Add(notification);

        return notifications;
    }
}