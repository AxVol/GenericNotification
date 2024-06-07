using GenericNotification.Domain.Interfaces;

namespace GenericNotification.Domain.Entity;

public class NotificationStatus : IEntityId<long>
{
    public long Id { get; set; }
    public string Email { get; set; }
    public bool SendStatus { get; set; }
    public Notification Notification { get; set; }
    public long NotificationId { get; set; }
}