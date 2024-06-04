using GenericNotification.Domain.Entity;
using GenericNotification.Domain.Enum;
using GenericNotification.Domain.Interfaces;

namespace GenericNotification.Domain.Response;

public class NotificationResponse : IResponse<Notification>
{
    public string Message { get; set; }
    public ResponseStatus Status { get; set; }
    public Notification Value { get; set; }
}