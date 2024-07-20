using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using NotificationSender.DAL.Repository.Interfaces;
using NotificationSender.Domain.Entity;
using NotificationSender.Domain.Resources;

namespace NotificationSender.DAL.Repository.Implementations;

public class NotificationRepository : IRepository<Notification>
{
    private readonly IDistributedCache redis;
    private readonly IStringLocalizer<Resources> localizationMessages;

    public NotificationRepository(IDistributedCache cache, IStringLocalizer<Resources> localization)
    {
        redis = cache;
        localizationMessages = localization;
    }
    
    public async Task AddAsync(Notification entity)
    {
        Dictionary<string, Notification> notifications = new Dictionary<string, Notification>();
        string notificationDate = entity.TimeToSend.ToString();
        string? notificationsJson = await redis.GetStringAsync(notificationDate);

        if (notificationsJson is not null)
            notifications = JsonSerializer.Deserialize<Dictionary<string, Notification>>(notificationsJson);

        notifications.Add(entity.Id.ToString(), entity);
        
        string notificationsDict = JsonSerializer.Serialize(notifications);
        await redis.SetStringAsync(notificationDate, notificationsDict);
    }

    public async Task<Notification> GetAsync(Notification entity)
    {
        Notification notification = new Notification();
        Dictionary<string, Notification> notifications = new Dictionary<string, Notification>();
        string id = entity.Id.ToString();
        string notificationDate = entity.TimeToSend.ToString();
        string? notificationsJson = await redis.GetStringAsync(notificationDate);

        if (notificationsJson is null)
            throw new InvalidDataException(localizationMessages["NotFound"]);
        
        notifications = JsonSerializer.Deserialize<Dictionary<string, Notification>>(notificationsJson);

        if (notifications.ContainsKey(id))
        {
            notification = notifications[entity.Id.ToString()];

            return notification;
        }
        else
        {
            throw new InvalidDataException(localizationMessages["NotFound"]);
        }
        
    }

    public async Task UpdateAsync(Notification entity)
    {
        Dictionary<string, Notification> notifications = new Dictionary<string, Notification>();
        string id = entity.Id.ToString();
        string notificationDate = entity.TimeToSend.ToString();
        string? notificationsJson = await redis.GetStringAsync(notificationDate);

        if (notificationsJson is null)
            throw new InvalidDataException(localizationMessages["NotFound"]);
        
        notifications = JsonSerializer.Deserialize<Dictionary<string, Notification>>(notificationsJson);
        
        if (notifications.ContainsKey(id))
        {
            notifications[id] = entity;
            string notificationsDict = JsonSerializer.Serialize(notifications);

            await redis.RemoveAsync(notificationDate);
            await redis.SetStringAsync(notificationDate, notificationsDict);
        }
        else
        {
            throw new InvalidDataException(localizationMessages["NotFound"]);
        }
    }

    public async Task DeleteAsync(Notification entity)
    {
        Dictionary<string, Notification> notifications = new Dictionary<string, Notification>();
        string id = entity.Id.ToString();
        string notificationDate = entity.TimeToSend.ToString();
        string? notificationsJson = await redis.GetStringAsync(notificationDate);

        if (notificationsJson is null)
            throw new InvalidDataException(localizationMessages["NotFound"]);
        
        notifications = JsonSerializer.Deserialize<Dictionary<string, Notification>>(notificationsJson);
        
        if (notifications.ContainsKey(id))
        {
            notifications.Remove(id);

            await redis.RemoveAsync(notificationDate);
            
            if (notifications.Count != 0)
            {
                string notificationsDict = JsonSerializer.Serialize(notifications);
                await redis.SetStringAsync(notificationDate, notificationsDict);
            }
        }
        else
        {
            throw new InvalidDataException(localizationMessages["NotFound"]);
        }
    }
}