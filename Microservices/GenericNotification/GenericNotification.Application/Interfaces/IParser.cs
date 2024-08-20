using GenericNotification.Domain.Entity;
using Microsoft.AspNetCore.Http;

namespace GenericNotification.Application.Interfaces;

public interface IParser
{
    public Dictionary<string, string> FileExtensions { get; }
    public List<NotificationStatus> Parse(IFormFile file);
    public List<NotificationStatus> Parse(string text);
}