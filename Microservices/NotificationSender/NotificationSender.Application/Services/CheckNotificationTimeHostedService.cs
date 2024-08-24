using System.Net;
using System.Net.Http.Json;
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
    private const string Url = "https://localhost:7125/api";

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

        // Приводиться время к общему виду которое хранит redis отбросив секунды
        int seconds = 0;
        DateTime dateTime = new DateTime(
            DateTime.UtcNow.Year,
            DateTime.UtcNow.Month,
            DateTime.UtcNow.Day,
            DateTime.UtcNow.Hour,
            DateTime.UtcNow.Minute,
            seconds
        );
        Dictionary<string, Notification> notifications = await repository.GetAllByDate(dateTime);

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
                return;
            }
            await repository.DeleteAsync(notification);
            await DeleteNotificationFromDb(notification.Id);
        }
        await repository.DeleteByDate(dateTime);
    }

    private async Task DeleteNotificationFromDb(Guid id)
    {
        string deleteUrl = $"{Url}/Notification/{id.ToString()}";

        using (HttpClient client = new HttpClient())
        {
            var response = await client.DeleteAsync(deleteUrl);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string? error = await response.Content.ReadFromJsonAsync<string>();
                
                logger.LogError(error);
            }
        }
    }
}