using NotificationSender.Domain.Interfaces;

namespace NotificationSender.Domain.Entity;

public class NotificationStatus : IEntityId<Guid>
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public bool SendStatus { get; set; }
}