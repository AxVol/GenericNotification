using System.Text.Json;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using NotificationSender.Application.Interfaces;
using NotificationSender.Consumer.Interfaces;
using NotificationSender.DAL.Repository.Interfaces;
using NotificationSender.Domain.Emun;
using NotificationSender.Domain.Entity;
using NotificationSender.Domain.Resources;

namespace NotificationSender.Application.Services;

public class ConsumerService : IConsumerService
{
    private readonly IConsumer rabbit;
    private readonly IRepository<Notification> repository;
    private readonly ILogger<ConsumerService> logger;
    private readonly IStringLocalizer<Resources> localizer;

    public ConsumerService(IRepository<Notification> repo, IConsumer consumer, ILogger<ConsumerService> log,
        IStringLocalizer<Resources> local)
    {
        rabbit = consumer;
        repository = repo;
        logger = log;
        localizer = local;
        
        rabbit.SubscribeAsync(NotificationCallback);
    }
    
    public async Task<bool> NotificationCallback(string message)
    {
        Notification? notification = JsonSerializer.Deserialize<Notification>(message);

        if (notification is null)
        {
            logger.LogError(localizer["NotificationQueueError"]);
            return false;
        }

        notification.SetNotificaitonCount(notification.ForUsers.Count);
        notification.NotificationState = NotificationState.NotStarted;
        logger.LogInformation($"Notification Added, uuid - {notification.Id}");
        await repository.AddAsync(notification);

        return true;
    }
}