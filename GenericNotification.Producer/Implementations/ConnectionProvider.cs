using GenericNotification.Producer.Interfaces;
using RabbitMQ.Client;

namespace GenericNotification.Producer.Implementations;

public class ConnectionProvider : IConnectionProvider
{
    public ConnectionProvider(string connectionString)
    {
        
    }

    public IConnection GetConnection()
    {
        throw new NotImplementedException();
    }
    
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}