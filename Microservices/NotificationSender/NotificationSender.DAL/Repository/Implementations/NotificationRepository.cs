using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using NotificationSender.DAL.Repository.Interfaces;
using NotificationSender.Domain.Entity;
using NotificationSender.Domain.Resources;

namespace NotificationSender.DAL.Repository.Implementations;

public class NotificationRepository : INotificationRepository
{
    private readonly IDistributedCache redis;
    private readonly IStringLocalizer<Resources> localizationMessages;

    public NotificationRepository(IDistributedCache cache, IStringLocalizer<Resources> localization)
    {
        redis = cache;
        localizationMessages = localization;
    }

    public async Task DeleteByDate(DateTime dateTime)
    {
        await redis.RemoveAsync(dateTime.ToString());
    }

    public async Task<Dictionary<string, Notification>> GetAllByDate(DateTime dateTime)
    {
        Dictionary<string, Notification> notifications = new Dictionary<string, Notification>();
        string notificationDate = dateTime.ToString();
        string? notificationsJson = await redis.GetStringAsync(notificationDate);

        if (notificationsJson is not null)
            notifications = JsonSerializer.Deserialize<Dictionary<string, Notification>>(notificationsJson);

        return notifications;
    }
    
    public async Task AddAsync(Notification entity)
    {
        Dictionary<string, Notification> notifications = await GetAllByDate(entity.TimeToSend);
        notifications.Add(entity.Id.ToString(), entity);
        
        string notificationsDict = JsonSerializer.Serialize(notifications);
        await redis.SetStringAsync(entity.TimeToSend.ToString(), notificationsDict);
    }

    public async Task<Notification> GetAsync(Notification entity)
    {
        Notification notification = new Notification();
        Dictionary<string, Notification> notifications = await GetAllByDate(entity.TimeToSend);
        string id = entity.Id.ToString();

        if (notifications.Count == 0)
            throw new InvalidDataException(localizationMessages["NotFound"]);

        if (notifications.ContainsKey(id))
        {
            notification = notifications[id];

            return notification;
        }
        else
        {
            throw new InvalidDataException(localizationMessages["NotFound"]);
        }
        
    }

    public async Task UpdateAsync(Notification entity)
    {
        Dictionary<string, Notification> notifications = await GetAllByDate(entity.TimeToSend);
        string id = entity.Id.ToString();
        string date = entity.TimeToSend.ToString();
        
        if (notifications.Count == 0)
            throw new InvalidDataException(localizationMessages["NotFound"]);

        if (notifications.ContainsKey(id))
        {
            notifications[id] = entity;
            string notificationsDict = JsonSerializer.Serialize(notifications);

            await redis.RemoveAsync(date);
            await redis.SetStringAsync(date, notificationsDict);
        }
        else
        {
            throw new InvalidDataException(localizationMessages["NotFound"]);
        }
    }

    public async Task DeleteAsync(Notification entity)
    {
        Dictionary<string, Notification> notifications = await GetAllByDate(entity.TimeToSend);
        string id = entity.Id.ToString();
        string date = entity.TimeToSend.ToString();
        
        if (notifications.Count == 0)
            throw new InvalidDataException(localizationMessages["NotFound"]);

        if (notifications.ContainsKey(id))
        {
            notifications.Remove(id);

            await redis.RemoveAsync(date);
            
            if (notifications.Count != 0)
            {
                string notificationsDict = JsonSerializer.Serialize(notifications);
                await redis.SetStringAsync(date, notificationsDict);
            }
        }
        else
        {
            throw new InvalidDataException(localizationMessages["NotFound"]);
        }
    }
}