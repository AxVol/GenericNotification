using GenericNotification.DAL.Repository.Interfaces;
using GenericNotification.Domain.Entity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericNotification.Application.Service;

/// <summary>
/// Бэкграунд сервис отвечающий за то, чтобы каждый день в 12 часов он проверял базу данных на ранее созданые нотификации
/// и отправлял их в сервис по отправке, при условии если есть нотификации которые надо отправить в течении текущего дня
/// </summary>
public class UpdateNotificationListHostedService : BackgroundService
{
    private readonly ILogger<UpdateNotificationListHostedService> logger;
    private readonly IRepository<Notification> repository;

    public UpdateNotificationListHostedService(ILogger<UpdateNotificationListHostedService> log,
        IServiceScopeFactory factory)
    {
        logger = log;
        repository = factory.CreateScope().ServiceProvider.GetRequiredService<IRepository<Notification>>();
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            TimeSpan waitTime = new TimeSpan(23, 59, 59) - DateTime.UtcNow.TimeOfDay;

            logger.LogInformation($"До следующего запуска осталось - {waitTime.ToString()}");
            await Task.Delay(waitTime, stoppingToken);
            
            await DoWork(null);
        } while (!stoppingToken.IsCancellationRequested);
    }

    private Task DoWork(object? state)
    {
        int counter = 0;
        DateTime today = DateTime.UtcNow;
        IEnumerable<Notification> notifications = repository.GetAll().Where(n => n.TimeToSend.Year == today.Year
        && n.TimeToSend.Month == today.Month && n.TimeToSend.Day == today.Day);

        foreach (Notification notification in notifications)
        {
            counter++;
            repository.AddToBrokerAsync(notification, "NotificationSend");
        }

        logger.LogInformation($"В очередь было загружено - {counter} нотификаций");
        
        return Task.CompletedTask;
    }
}