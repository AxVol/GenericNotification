using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationSender.Application.Interfaces;
using NotificationSender.DAL.Repository.Interfaces;
using NotificationSender.Domain.Entity;

namespace NotificationSender.Application.Services;

/// <summary>
/// 
/// </summary>
public class CheckNotificationTimeHostedService : BackgroundService
{
    private readonly ILogger<CheckNotificationTimeHostedService> logger;
    private readonly INotificationRepository repository;
    private readonly INotificationSenderService notificationSenderService;
    
    private const int IntervalInMinutes = 1;

    public CheckNotificationTimeHostedService(ILogger<CheckNotificationTimeHostedService> log, 
        IServiceScopeFactory factory)
    {
        logger = log;
        repository = factory.CreateScope().ServiceProvider.GetRequiredService<INotificationRepository>();
        notificationSenderService = factory.CreateScope().ServiceProvider.GetRequiredService<INotificationSenderService>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            await Task.Delay(TimeSpan.FromMinutes(IntervalInMinutes), stoppingToken);
            await DoWork(null);
        } while (!stoppingToken.IsCancellationRequested);
    }

    private async Task DoWork(object? state)
    {
        logger.LogInformation("Start checking notification");
        
        DateTime dateTime = DateTime.UtcNow;
        Dictionary<string, Notification> notifications = await repository.GetAllByDate(dateTime);
        await repository.DeleteByDate(dateTime);

        foreach (KeyValuePair<string, Notification> valuePair in notifications)
        {
            Notification notification = valuePair.Value;
            
            try
            {
                logger.LogInformation($"Sending notification with uuid - {notification.Id}");
                await notificationSenderService.SendNotificationAsync(notification);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}