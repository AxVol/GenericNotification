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

    public Queue<NotificationStatus> Parse(IFormFile file)
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
                notificationStatus = CsvParse(stream);
                break;
            }
            default:
            {
                throw new ArgumentException(localizer["FileParseError"]);
            }
        }

        return notificationStatus;
    }

    public Queue<NotificationStatus> Parse(string text)
    {
        throw new NotImplementedException();
    }

    private Queue<NotificationStatus> CsvParse(Stream stream)
    {
        throw new NotImplementedException();
    }

    private Queue<NotificationStatus> ExcelParse(Stream stream)
    {
        throw new NotImplementedException();
    }
}