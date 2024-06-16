using GenericNotification.DAL.Repository.Interfaces;
using GenericNotification.Domain.Entity;

namespace GenericNotification.DAL.Repository.Implementations;

public class NotificationStatusRepository : IDbRepository<NotificationStatus>
{
    private ApplicationDbContext db;

    public NotificationStatusRepository(ApplicationDbContext context)
    {
        db = context;
    }
    
    public async Task CreateAsync(NotificationStatus entity)
    {
        db.NotificationStatus.Add(entity);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(NotificationStatus entity)
    {
        db.NotificationStatus.Update(entity);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(NotificationStatus entity)
    {
        db.NotificationStatus.Remove(entity);
        await db.SaveChangesAsync();
    }

    public IEnumerable<NotificationStatus> GetAll()
    {
        return db.NotificationStatus;
    }
}