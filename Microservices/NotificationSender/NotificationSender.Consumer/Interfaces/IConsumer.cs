namespace NotificationSender.Consumer.Interfaces;

public interface IConsumer : IDisposable
{
    public void SubscribeAsync(Func<string, Task<bool>> callback);
}