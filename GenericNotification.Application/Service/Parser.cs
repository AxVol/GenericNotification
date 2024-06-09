using System.ComponentModel.DataAnnotations;
using GenericNotification.Application.Interfaces;
using GenericNotification.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;

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
        Queue<NotificationStatus> notificationQueue;
        using (StreamReader streamReader = new StreamReader(stream))
        {
            string line = streamReader.ReadToEnd();
            List<string> parts = line.Split(',').ToList();

            notificationQueue = GetEmails(parts);
        }

        return notificationQueue;
    }
    // TODO рефакторинг
    private Queue<NotificationStatus> ExcelParse(Stream stream)
    {
        Queue<NotificationStatus> notificationQueue;
        List<string> emails = new List<string>();
        
        using (ExcelPackage package = new ExcelPackage(stream))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
            int columnCount = worksheet.Dimension.End.Column;
            int rowCount = worksheet.Dimension.End.Row;
            
            if (columnCount == 0 && rowCount == 0) throw new ArgumentException(localizer["FileParseError"]);
            if (worksheet.Cells[1, 1] == null) throw new ArgumentException(localizer["FileParseError"]);

            /*
             * Проверяет то, как данные расположены внутри таблички, отслеживаются 3 состояния
             * 1 - Когда данные расположены только в первых ячейках каждого столбца
             * 2 - Когда данные расположены только в первом столбце
             * 3 - Когда данные расположены в виде полноценной таблицы
             */
            if (worksheet.Cells[2, 2] == null)
            {
                // Парсит только первый столбец
                if (columnCount == 1)
                {
                    for (int i = 1; i < rowCount; i++)
                    {
                        string current = worksheet.Cells[i, columnCount].Value.ToString();

                        if (current == null)
                        {
                            throw new ArgumentException(localizer["FileParseError"]);
                        }
                        else
                        {
                            emails.Add(current);
                        }
                    }
                }
                // Парсит только первую ячейку всех столбцов
                else if (rowCount == 1)
                {
                    for (int i = 1; i < columnCount; i++)
                    {
                        string current = worksheet.Cells[rowCount, i].Value.ToString();

                        if (current == null)
                        {
                            throw new ArgumentException(localizer["FileParseError"]);
                        }
                        else
                        {
                            emails.Add(current);
                        }
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
                        string current = worksheet.Cells[row, column].Value.ToString();

                        if (current == null)
                        {
                            throw new ArgumentException(localizer["FileParseError"]);
                        }
                        else
                        {
                            emails.Add(current);
                        }
                    }
                }
            }

            notificationQueue = GetEmails(emails);
        }

        return notificationQueue;
    }

    private Queue<NotificationStatus> GetEmails(List<string> emails)
    {
        Queue<NotificationStatus> notificationQueue = new Queue<NotificationStatus>();
        
        Parallel.ForEach(emails, current =>
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

        return notificationQueue;
    }
}