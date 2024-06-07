using System.ComponentModel.DataAnnotations;
using GenericNotification.Application.Interfaces;
using GenericNotification.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace GenericNotification.Application.Service;

public class Parser : IParser
{
    private readonly IStringLocalizer<Resources.Resources> localizer;
    public Dictionary<string, string> FileExtensions
    {
        get => new Dictionary<string, string>()
        {
            {"application/vnd.ms-excel", "excel"},
            {"application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "excel"},
            {"application/csv", "csv"},
            {"text/csv", "csv"}
        };
    }

    public Parser(IStringLocalizer<Resources.Resources> stringLocalizer)
    {
        localizer = stringLocalizer;
    }

    public Queue<NotificationStatus> Parse(IFormFile file, Notification notification)
    {
        Queue<NotificationStatus> notificationStatus;
        string fileType = FileExtensions[file.ContentType];
        Stream stream = file.OpenReadStream();

        switch (fileType)
        {
            case "excel":
            {
                notificationStatus = ExcelParse(stream, notification);
                break;
            }
            case "csv":
            {
                notificationStatus = CsvParse(stream, notification);
                break;
            }
            default:
            {
                throw new ArgumentException(localizer["FileParseError"]);
            }
        }

        return notificationStatus;
    }

    public Queue<NotificationStatus> Parse(string text, Notification notification)
    {
        throw new NotImplementedException();
    }

    private Queue<NotificationStatus> CsvParse(Stream stream, Notification notification)
    {
        Queue<NotificationStatus> notificationQueue = new Queue<NotificationStatus>();
        using (StreamReader streamReader = new StreamReader(stream))
        {
            string line = streamReader.ReadToEnd();
            string[] parts = line.Split(',');
            //TODO: Распаралелить
            for (int i = 0; i < parts.Length - 1; i++)
            {
                string current = parts[i];
                bool isEmail = new EmailAddressAttribute().IsValid(current);

                if (isEmail)
                {
                    NotificationStatus notificationStatus = new NotificationStatus()
                    {
                        Email = current,
                        SendStatus = false,
                        Notification = notification,
                        
                    };
                    notificationQueue.Enqueue(notificationStatus);
                }
                else
                {
                    throw new ArgumentException(localizer["FileParseError"]);
                }
            }
        }

        return notificationQueue;
    }

    private Queue<NotificationStatus> ExcelParse(Stream stream, Notification notification)
    {
        throw new NotImplementedException();
    }
}