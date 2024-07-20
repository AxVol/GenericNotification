using NotificationSender.Domain.Emun;
using NotificationSender.Domain.Interfaces;

namespace NotificationSender.Domain.Response;

public class UsersProcessedResponse : IResponse<int>
{
    public string Message { get; set; }
    public ResponseStatus Status { get; set; }
    public int Value { get; set; }
}