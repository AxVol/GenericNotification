using GenericNotification.Producer.Interfaces;
using RabbitMQ.Client;

namespace GenericNotification.Producer.Implementations;

public class ConnectionProvider : IConnectionProvider
{
    private ConnectionFactory factory;
    private IConnection connection;
    
    public ConnectionProvider(string connectionString)
    {
        factory = new ConnectionFactory
        {
            Uri = new Uri(connectionString)
        };

        connection = factory.CreateConnection();
    }

    public IConnection GetConnection()
    {
        return connection;
    }
    
    public void Dispose()
    {
        connection.Close();
        GC.SuppressFinalize(this);
    }
}