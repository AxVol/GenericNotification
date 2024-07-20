using NotificationSender.Domain.Emun;

namespace NotificationSender.Domain.Interfaces;

public interface IResponse<T>
{
    public string Message { get; set; }
    public ResponseStatus Status { get; set; }
    public T Value { get; set; }
}