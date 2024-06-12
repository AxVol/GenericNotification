namespace GenericNotification.Producer.Interfaces;

public interface IProducer : IDisposable
{
    void Publish(string message, string routingKey);
}