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
        model.QueueDeclare(queue, true, false, false);
        model.ExchangeDeclare(exchange, ExchangeType.Direct, true, false);
        model.QueueBind(queue, exchange, routingKey);
    }

    public async Task SubscribeAsync(Func<string, Task<bool>> callback)
    {
        AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(model);
        
        consumer.Received += async (ch, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            string text = Encoding.UTF8.GetString(body);
            bool result = await callback.Invoke(text);

            if (result)
            {
                model.BasicAck(ea.DeliveryTag, false);
            }
        };
        
        model.BasicConsume(queue, false, consumer);
    }

    public void Dispose()
    {
        model.Close();
        GC.SuppressFinalize(this);
    }
}