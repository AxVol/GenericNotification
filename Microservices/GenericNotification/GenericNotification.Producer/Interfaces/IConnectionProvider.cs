using RabbitMQ.Client;

namespace GenericNotification.Producer.Interfaces;

public interface IConnectionProvider : IDisposable
{
    public IConnection GetConnection();
}
