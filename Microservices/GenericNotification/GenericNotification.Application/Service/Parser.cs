using System.ComponentModel.DataAnnotations;
using GenericNotification.Application.Interfaces;
using GenericNotification.Domain.Entity;
using GenericNotification.Domain.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;

namespace GenericNotification.Application.Service;

public class Parser : IParser
{
    private readonly IStringLocalizer<Resources> localizer;

    public Dictionary<string, string> FileExtensions { get; }

    public Parser(IStringLocalizer<Resources> stringLocalizer)
    {
        localizer = stringLocalizer;
        FileExtensions = new Dictionary<string, string>()
        {
            { "application/vnd.ms-excel", "excel" },
            { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "excel" },
            { "application/csv", "csv" },
            { "text/csv", "csv" }
        };
    }

    public List<NotificationStatus> Parse(IFormFile file)
    {
        List<NotificationStatus> notificationStatus;
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

    public List<NotificationStatus> Parse(string text)
    {
        List<string> parts = text.Split(' ').ToList();
        List<NotificationStatus> notificationList = GetEmails(parts);

        return notificationList;
    }

    private List<NotificationStatus> CsvParse(Stream stream)
    {
        List<NotificationStatus> notificationList;
        using (StreamReader streamReader = new StreamReader(stream))
        {
            string line = streamReader.ReadToEnd();
            List<string> parts = line.Split(',').ToList();

            notificationList = GetEmails(parts);
        }

        return notificationList;
    }
    // TODO рефакторинг
    private List<NotificationStatus> ExcelParse(Stream stream)
    {
        List<NotificationStatus> notificationList;
        List<string> emails = new List<string>();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (ExcelPackage package = new ExcelPackage(stream))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            int columnCount = worksheet.Dimension.End.Column;
            int rowCount = worksheet.Dimension.End.Row;
            
            if (columnCount == 0 && rowCount == 0) throw new ArgumentException(localizer["FileParseError"]);

            /*
             * Проверяет то, как данные расположены внутри таблички, отслеживаются 3 состояния
             * 1 - Когда данные расположены только в первых ячейках каждого столбца
             * 2 - Когда данные расположены только в первом столбце
             * 3 - Когда данные расположены в виде полноценной таблицы
             */
            if (worksheet.Cells[2, 2].Value == null)
            {
                // Парсит только первый столбец
                if (columnCount == 1)
                {
                    for (int i = 1; i <= rowCount; i++)
                    {
                        string current = worksheet.Cells[i, columnCount].Value.ToString();
                        emails.Add(current);
                    }
                }
                // Парсит только первую ячейку всех столбцов
                else if (rowCount == 1)
                {
                    for (int i = 1; i <= columnCount; i++)
                    {
                        string current = worksheet.Cells[rowCount, i].Value.ToString();
                        emails.Add(current);
                    }
                }
                else
                {
                    throw new ArgumentException(localizer["FileParseError"]);
                }
            }
            else
            {
                // Парсит полноценную таблицу
                for (int row = 1; row <= rowCount; row++)
                {
                    for (int column = 1; column <= columnCount; column++)
                    {
                        var current = worksheet.Cells[row, column].Value;
                        
                        if (current is null)
                            continue;
                        
                        emails.Add(current.ToString());
                    }
                }
            }

            notificationList = GetEmails(emails);
        }

        return notificationList;
    }

    private List<NotificationStatus> GetEmails(List<string> emails)
    {
        List<NotificationStatus> notificationList = new List<NotificationStatus>();
        
        Parallel.ForEach(emails, current =>
        {
            bool isEmail = new EmailAddressAttribute().IsValid(current);

            if (isEmail)
            {
                NotificationStatus notificationStatus = new NotificationStatus()
                {
                    Id = Guid.NewGuid(),
                    Email = current,
                    SendStatus = false,
                };
                notificationList.Add(notificationStatus);
            }
            else
            {
                throw new ArgumentException(localizer["FileParseError"]);
            }
        });

        return notificationList;
    }
}