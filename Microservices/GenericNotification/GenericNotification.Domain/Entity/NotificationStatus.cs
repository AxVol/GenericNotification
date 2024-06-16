using System.ComponentModel.DataAnnotations;
using GenericNotification.Domain.Interfaces;

namespace GenericNotification.Domain.Entity;

public class NotificationStatus : IEntityId<Guid>
{
    [Key]
    public Guid Id { get; set; }
    public string Email { get; set; }
    public bool SendStatus { get; set; }
}