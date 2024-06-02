using GenericNotification.Domain.Interfaces;

namespace GenericNotification.Domain.Entity;

public class Notification : IEntityId<long>
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime TimeToSend { get; set; }
    public bool IsSend { get; set; }
    public Queue<NotificationStatus> ForUsers { get; set; }
    public string CreatorName { get; set; }
}