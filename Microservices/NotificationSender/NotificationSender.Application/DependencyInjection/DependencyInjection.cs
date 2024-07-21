using Microsoft.Extensions.DependencyInjection;
using NotificationSender.Application.Interfaces;
using NotificationSender.Application.Services;

namespace NotificationSender.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IConsumerService, ConsumerService>();
        serviceCollection.AddTransient<INotificationService, NotificationService>();
        serviceCollection.AddHostedService<CheckNotificationTimeHostedService>();

        return serviceCollection;
    }
}