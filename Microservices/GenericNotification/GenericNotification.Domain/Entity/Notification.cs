using System.ComponentModel.DataAnnotations;
using GenericNotification.Domain.Interfaces;

namespace GenericNotification.Domain.Entity;

public class Notification : IEntityId<Guid>
{
    [Key]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime TimeToSend { get; set; }
    public bool IsSend { get; set; }
    public List<NotificationStatus> ForUsers { get; set; }
    public string CreatorName { get; set; }
}