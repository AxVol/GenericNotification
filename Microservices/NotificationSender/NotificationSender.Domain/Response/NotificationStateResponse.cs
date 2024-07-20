using NotificationSender.Domain.Emun;
using NotificationSender.Domain.Interfaces;

namespace NotificationSender.Domain.Response;

public class NotificationStateResponse : IResponse<string>
{
    public string Message { get; set; }
    public ResponseStatus Status { get; set; }
    public string Value { get; set; }
}