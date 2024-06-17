using System.Text;
using NotificationSender.Consumer.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NotificationSender.Consumer.Implementations;

public class Consumer : IConsumer
{
    private readonly IConnectionProvider connectionProvider;
    private readonly string exchange;
    private readonly string queue;
    private readonly IModel model;

    public Consumer(IConnectionProvider connection, string ex, string q, string routingKey)
    {
        connectionProvider = connection;
        exchange = ex;
        queue = q;
        
        model = connectionProvider.GetConnection().CreateModel();
        model.QueueBind(queue, exchange, routingKey);
    }

    public void SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callback)
    {
        AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(model);

        consumer.Received += async (sender, e) =>
        {
            byte[] body = e.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            bool success = await callback.Invoke(message, e.BasicProperties.Headers);

            if (success)
            {
                model.BasicAck(e.DeliveryTag, true);
            }
        };

        model.BasicConsume(queue, true, consumer);
    }
    
    public void Dispose()
    {
        model.Close();
        GC.SuppressFinalize(this);
    }
}