using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationSender.Consumer.Implementations;
using NotificationSender.Consumer.Interfaces;

namespace NotificationSender.Consumer.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBrokerConsumer(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("RabbitMQ");
        string exchange = configuration["Exchange"];
        string queue = configuration["Queue"];
        string routingKey = configuration["RoutingKey"];
        
        serviceCollection.AddSingleton<IConnectionProvider>(new ConnectionProvider(connectionString));
        serviceCollection.AddSingleton<IConsumer>(p => new Implementations.Consumer(
            p.GetService<IConnectionProvider>(), exchange, queue, routingKey));

        return serviceCollection;
    }
}