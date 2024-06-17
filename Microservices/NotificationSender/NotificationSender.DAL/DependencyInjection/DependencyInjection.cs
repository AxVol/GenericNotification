using Microsoft.Extensions.DependencyInjection;
using NotificationSender.DAL.Repository.Interfaces;

namespace NotificationSender.DAL.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLevel(this IServiceCollection serviceCollection)
    {
        

        return serviceCollection;
    }
}