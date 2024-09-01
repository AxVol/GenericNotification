using GenericNotification.Producer.Implementations;
using GenericNotification.Producer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace GenericNotification.Producer.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBrokerProducer(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("RabbitMQ");
        string exchange = configuration["Exchange"];
        string exchangeType = ExchangeType.Direct.ToString();

        serviceCollection.AddSingleton<IConnectionProvider>(new ConnectionProvider(connectionString));
        serviceCollection.AddScoped<IProducer>(p => new Implementations.Producer(p.GetService<IConnectionProvider>(),
            exchange, exchangeType));
        
        return serviceCollection;
    }
}