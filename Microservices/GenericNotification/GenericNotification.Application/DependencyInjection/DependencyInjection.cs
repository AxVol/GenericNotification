using GenericNotification.Application.Interfaces;
using GenericNotification.Application.Service;
using Microsoft.Extensions.DependencyInjection;

namespace GenericNotification.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<INotificationService, NotificationService>();
        serviceCollection.AddScoped<IParser, Parser>();
        serviceCollection.AddHostedService<UpdateNotificationListHostedService>();

        return serviceCollection;
    }
}
