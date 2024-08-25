using NotificationSender.Domain.Emun;
using NotificationSender.Domain.Interfaces;

namespace NotificationSender.Domain.Entity;

public class Notification : IEntityId<Guid>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime TimeToSend { get; set; }
    public List<NotificationStatus> ForUsers { get; set; }
    public string CreatorName { get; set; }
    public int CountNotifications { get; set; }
    public NotificationState NotificationState { get; set; }
}