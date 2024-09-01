using System.Text;
using System.Text.Json;
using GenericNotification.Producer.Interfaces;
using RabbitMQ.Client;

namespace GenericNotification.Producer.Implementations;

public class Producer : IProducer
{
    private readonly IConnectionProvider connectionProvider;
    private readonly string exchange;
    private readonly IModel model;

    public Producer(IConnectionProvider conProvider, string ex, string exchangeType, int ttl = 30000)
    {
        connectionProvider = conProvider;
        exchange = ex;
        model = connectionProvider.GetConnection().CreateModel();
        Dictionary<string, object> arg = new Dictionary<string, object>()
        {
            {"x-message-ttl", ttl }
        };

        model.ExchangeDeclare(exchange, exchangeType, arguments: arg, durable: true);
    }
    
    public async Task Publish<T>(T obj, string routingKey, string ttl = "30000")
    {
        await Task.Run(() =>
        {
            string json = JsonSerializer.Serialize(obj);
            byte[] body = Encoding.UTF8.GetBytes(json);
            IBasicProperties properties = model.CreateBasicProperties();
            properties.Persistent = true;
            properties.Expiration = ttl;
        
            model.BasicPublish(exchange, routingKey, properties, body);
        });
    }
    
    public void Dispose()
    {
        model.Close();
        GC.SuppressFinalize(this);
    }
}