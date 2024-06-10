using GenericNotification.Application.Interfaces;
using GenericNotification.Application.Resources;
using GenericNotification.Application.Service;
using GenericNotification.DAL.Repository;
using GenericNotification.Domain.Entity;
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
        IEnumerable<Notification> repositories = new List<Notification>();
        var mock = new Mock<IRepository<Notification>>();

        mock.Setup(m => m.Create(It.IsAny<Notification>())).Returns(Task.CompletedTask);
        mock.Setup(m => m.Delete(It.IsAny<Notification>())).Returns(Task.CompletedTask);
        mock.Setup(m => m.Update(It.IsAny<Notification>())).Returns(Task.CompletedTask);
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
}