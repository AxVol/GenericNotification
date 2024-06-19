namespace NotificationSender.Application.Interfaces;

public interface IConsumerService
{
    public Task<bool> NotificationCallback(string message);
}