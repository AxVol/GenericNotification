using GenericNotification.Producer.Interfaces;
using RabbitMQ.Client;

namespace GenericNotification.Producer.Implementations;

public class Producer : IProducer
{
    private readonly IConnectionProvider connectionProvider;
    private readonly string exchange;
    private readonly IModel model;

    public Producer(IConnectionProvider conProvider, string ex)
    {
        connectionProvider = conProvider;
        exchange = ex;
        model = connectionProvider.GetConnection().CreateModel();
    }
    
    public void Publish(string message, string routingKey)
    {
        throw new NotImplementedException();
    }
    
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}