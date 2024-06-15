using GenericNotification.DAL.Repository.Interfaces;
using GenericNotification.Domain.Entity;
using GenericNotification.Producer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GenericNotification.DAL.Repository.Implementations;

public class NotificationRepository : IRepository<Notification>
{
    private readonly ApplicationDbContext db;
    private readonly IProducer rabbit;
    
    public NotificationRepository(ApplicationDbContext context, IProducer producer)
    {
        db = context;
        rabbit = producer;
    }
    
    public async Task CreateAsync(Notification entity)
    {
        db.Notifications.Add(entity);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Notification entity)
    {
        db.Notifications.Update(entity);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Notification entity)
    {
        db.Notifications.Remove(entity);
        await db.SaveChangesAsync();
    }

    public IEnumerable<Notification> GetAll()
    {
        return db.Notifications.Include(n => n.ForUsers);
    }

    public async Task AddToBrokerAsync(Notification entity, string routingKey)
    { 
        await rabbit.Publish<Notification>(entity, routingKey);
    }
}