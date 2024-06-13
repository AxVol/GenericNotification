namespace GenericNotification.Producer.Interfaces;

public interface IProducer : IDisposable
{
    Task Publish(string message, string routingKey);
}