using RabbitMQ.Client;

namespace NotificationSender.Consumer.Interfaces;

public interface IConnectionProvider : IDisposable
{
    public IConnection GetConnection();
}