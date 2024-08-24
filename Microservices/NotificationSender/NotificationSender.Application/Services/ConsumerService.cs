using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NotificationSender.Application.Interfaces;
using NotificationSender.Consumer.Interfaces;
using NotificationSender.DAL.Repository.Interfaces;
using NotificationSender.Domain.Emun;
using NotificationSender.Domain.Entity;
using NotificationSender.Domain.Resources;

namespace NotificationSender.Application.Services;

public class ConsumerService : BackgroundService, IConsumerService
{
    private readonly IConsumer rabbit;
    private readonly INotificationRepository repository;
    private readonly ILogger<ConsumerService> logger;
    private readonly IStringLocalizer<Resources> localizer;

    public ConsumerService(INotificationRepository repo, IConsumer consumer, ILogger<ConsumerService> log,
        IStringLocalizer<Resources> local)
    {
        rabbit = consumer;
        repository = repo;
        logger = log;
        localizer = local;
    }
    
    public async Task<bool> NotificationCallback(string message)
    {
        Notification? notification = JsonSerializer.Deserialize<Notification>(message);

        if (notification is null)
        {
            logger.LogError(localizer["NotificationQueueError"]);
            return false;
        }

        notification.CountNotifications = notification.ForUsers.Count;
        notification.NotificationState = NotificationState.NotStarted;
        logger.LogInformation($"Notification Added, uuid - {notification.Id}");

        try
        {
            await repository.AddAsync(notification);
        }
        catch (Exception ex)
        {
            logger.LogError($"{localizer["AddToRedisError"]}\n Error - {ex.Message} ");
        }

        return true;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        rabbit.SubscribeAsync(NotificationCallback);
        
        return Task.CompletedTask;
    }
}