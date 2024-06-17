namespace NotificationSender.Consumer.Interfaces;

public interface IConsumer : IDisposable
{
    public void SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callback);
}