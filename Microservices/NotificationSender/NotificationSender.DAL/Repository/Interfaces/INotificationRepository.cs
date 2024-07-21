using NotificationSender.Domain.Entity;

namespace NotificationSender.DAL.Repository.Interfaces;

public interface INotificationRepository : IRepository<Notification>
{
    public Task<Dictionary<string, Notification>> GetAllByDate(DateTime dateTime);
}