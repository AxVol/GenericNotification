using GenericNotification.Producer.Implementations;
using GenericNotification.Producer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace GenericNotification.Producer.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBrokerProducer(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("RabbitMQ");

        serviceCollection.AddSingleton<IConnectionProvider>(new ConnectionProvider(connectionString));
        serviceCollection.AddScoped<IProducer>(p => new Implementations.Producer(p.GetService<IConnectionProvider>(),
            "NotificationExchange"));
        
        return serviceCollection;
    }
}