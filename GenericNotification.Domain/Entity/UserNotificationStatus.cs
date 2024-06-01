using GenericNotification.Domain.Interfaces;

namespace GenericNotification.Domain.Entity;

public class UserNotificationStatus : IEntityId<long>
{
    public long Id { get; set; }
    public string PhoneNumber { get; set; }
    public bool SendStatus { get; set; }
}