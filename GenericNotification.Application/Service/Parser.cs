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

    public async Task<Queue<NotificationStatus>> ParseAsync(IFormFile file)
    {
        Queue<NotificationStatus> notificationStatus;
        string fileType = FileExtensions[file.ContentType];
        Stream stream = file.OpenReadStream();

        switch (fileType)
        {
            case "excel":
            {
                notificationStatus = ExcelParse(stream);
                break;
            }
            case "csv":
            {
                notificationStatus = await CsvParse(stream);
                break;
            }
            default:
            {
                throw new ArgumentException(localizer["FileParseError"]);
            }
        }

        return notificationStatus;
    }

    public Task<Queue<NotificationStatus>> ParseAsync(string text)
    {
        throw new NotImplementedException();
    }

    private Task<Queue<NotificationStatus>> CsvParse(Stream stream)
    {
        Queue<NotificationStatus> notificationQueue = new Queue<NotificationStatus>();
        using (StreamReader streamReader = new StreamReader(stream))
        {
            string line = streamReader.ReadToEnd();
            string[] parts = line.Split(',');

            Parallel.ForEach(parts, current =>
            {
                bool isEmail = new EmailAddressAttribute().IsValid(current);

                if (isEmail)
                {
                    NotificationStatus notificationStatus = new NotificationStatus()
                    {
                        Email = current,
                        SendStatus = false,
                    };
                    notificationQueue.Enqueue(notificationStatus);
                }
                else
                {
                    throw new ArgumentException(localizer["FileParseError"]);
                }
            });
        }

        return Task.FromResult(notificationQueue);
    }

    private Queue<NotificationStatus> ExcelParse(Stream stream)
    {
        throw new NotImplementedException();
    }
}