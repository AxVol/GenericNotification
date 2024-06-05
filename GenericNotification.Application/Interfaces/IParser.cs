using GenericNotification.Domain.Entity;
using Microsoft.AspNetCore.Http;

namespace GenericNotification.Application.Interfaces;

public interface IParser
{
    public Dictionary<string, string> FileExtensions { get; }

    public Queue<NotificationStatus> Parse(IFormFile file);
    public Queue<NotificationStatus> Parse(string text);
}