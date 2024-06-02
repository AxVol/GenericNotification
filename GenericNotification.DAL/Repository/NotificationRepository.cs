using GenericNotification.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace GenericNotification.DAL.Repository;

public class NotificationRepository : IRepository<Notification>
{
    private readonly ApplicationDbContext db;
    
    public NotificationRepository(ApplicationDbContext context)
    {
        db = context;
    }
    
    public async Task Create(Notification entity)
    {
        db.Notifications.Add(entity);
        await db.SaveChangesAsync();
    }

    public async Task Update(Notification entity)
    {
        db.Notifications.Update(entity);
        await db.SaveChangesAsync();
    }

    public async Task Delete(Notification entity)
    {
        db.Notifications.Remove(entity);
        await db.SaveChangesAsync();
    }

    public IEnumerable<Notification> GetAll()
    {
        return db.Notifications.Include(n => n.ForUsers);
    }
}