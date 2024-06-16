using GenericNotification.Domain.Enum;

namespace GenericNotification.Domain.Interfaces;

public interface IResponse<T> where T : class 
{
    public string Message { get; set; }
    public ResponseStatus Status { get; set; }
    public T Value { get; set; }
}