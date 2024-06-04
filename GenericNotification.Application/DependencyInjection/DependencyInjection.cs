using GenericNotification.Application.Service;
using Microsoft.Extensions.DependencyInjection;

namespace GenericNotification.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<NotificationService>();
        
        return serviceCollection;
    }
}
