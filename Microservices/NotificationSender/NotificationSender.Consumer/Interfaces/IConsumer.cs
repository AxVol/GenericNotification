namespace NotificationSender.Consumer.Interfaces;

public interface IConsumer : IDisposable
{
    public Task SubscribeAsync(Func<string, Task<bool>> callback);
}