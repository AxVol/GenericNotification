using GenericNotification.Domain.Entity;

namespace GenericNotification.DAL.Repository;

public class NotificationStatusRepository : IRepository<NotificationStatus>
{
    private ApplicationDbContext db;

    public NotificationStatusRepository(ApplicationDbContext context)
    {
        db = context;
    }
    
    public async Task Create(NotificationStatus entity)
    {
        db.NotificationStatus.Add(entity);
        await db.SaveChangesAsync();
    }

    public async Task Update(NotificationStatus entity)
    {
        db.NotificationStatus.Update(entity);
        await db.SaveChangesAsync();
    }

    public async Task Delete(NotificationStatus entity)
    {
        db.NotificationStatus.Remove(entity);
        await db.SaveChangesAsync();
    }

    public IEnumerable<NotificationStatus> GetAll()
    {
        return db.NotificationStatus;
    }
}