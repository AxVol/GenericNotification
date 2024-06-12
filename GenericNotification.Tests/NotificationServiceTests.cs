using GenericNotification.Application.Interfaces;
using GenericNotification.Application.Resources;
using GenericNotification.Application.Service;
using GenericNotification.DAL.Repository;
using GenericNotification.Domain.DTO;
using GenericNotification.Domain.Entity;
using GenericNotification.Domain.Enum;
using GenericNotification.Domain.Response;
using GenericNotification.Tests.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Xunit;

namespace GenericNotification.Tests;

public class NotificationServiceTests
{
    private readonly INotificationService notificationService;
    private IStringLocalizer<Resources> localizer;
    
    public NotificationServiceTests()
    {
        ILogger<NotificationService> logger = Mock.MockFactory<NotificationService>.GetLogger();
        IRepository<Notification> repository = Mock.MockFactory<NotificationService>.GetNotificationRepository();
        IParser parser = Mock.MockFactory<NotificationService>.GetParser();
        localizer = Mock.MockFactory<NotificationService>.GetLocalaizer();

        notificationService = new NotificationService(localizer, parser, logger, repository);
    }

    [Fact]
    public async void Create_Correct_With_Body_Text_Test()
    {
        // Arrage
        DateTime dateTime = DateTime.Now.AddHours(1);
        NotificationDto dto = new NotificationDto()
        {
            Creator = "123",
            Title = "123",
            TimeToSend = dateTime,
            SenderEmail = "123@mail.ru",
            NotificationText = "1223",
            Body = "test1@mail.ru test2@mail.ru test3@mail.ru test4@mail.ru test5@mail.ru test6@mail.ru test7@mail.ru",
            File = null,
        };

        // Act
        NotificationResponse response = await notificationService.CreateNotificationAsync(dto);

        // Assert
        Assert.Equal(ResponseStatus.Success, response.Status);
        Assert.Equal(7, response.Value.ForUsers.Count);
    }

    [Fact]
    public async void Create_Correct_With_Horizontal_Excel_Test()
    {
        // Arrage
        FormFile file = FileUtils.GetFormFile("Files/ExcelHorizontalTest.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        DateTime dateTime = DateTime.Now.AddHours(1);

        NotificationDto dto = new NotificationDto()
        {
            Creator = "123",
            Title = "123",
            TimeToSend = dateTime,
            SenderEmail = "123@mail.ru",
            NotificationText = "1223",
            Body = null,
            File = file
        };

        // Act
        NotificationResponse response = await notificationService.CreateNotificationAsync(dto);

        // Assert
        Assert.Equal(ResponseStatus.Success, response.Status);
        Assert.Equal(10, response.Value.ForUsers.Count);
    }
    
    [Fact]
    public async void Create_Correct_With_Vertical_Excel_Test()
    {
        // Arrage
        FormFile file = FileUtils.GetFormFile("Files/ExcelVerticalTest.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        DateTime dateTime = DateTime.Now.AddHours(1);

        NotificationDto dto = new NotificationDto()
        {
            Creator = "123",
            Title = "123",
            TimeToSend = dateTime,
            SenderEmail = "123@mail.ru",
            NotificationText = "1223",
            Body = null,
            File = file
        };

        // Act
        NotificationResponse response = await notificationService.CreateNotificationAsync(dto);

        // Assert
        Assert.Equal(ResponseStatus.Success, response.Status);
        Assert.Equal(10, response.Value.ForUsers.Count);
    }
    
    [Fact]
    public async void Create_Correct_With_Table_Excel_Test()
    {
        // Arrage
        FormFile file = FileUtils.GetFormFile("Files/ExcelTableTest.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        DateTime dateTime = DateTime.Now.AddHours(1);

        NotificationDto dto = new NotificationDto()
        {
            Creator = "123",
            Title = "123",
            TimeToSend = dateTime,
            SenderEmail = "123@mail.ru",
            NotificationText = "1223",
            Body = null,
            File = file
        };

        // Act
        NotificationResponse response = await notificationService.CreateNotificationAsync(dto);

        // Assert
        Assert.Equal(ResponseStatus.Success, response.Status);
        Assert.Equal(9, response.Value.ForUsers.Count);
    }

    [Fact]
    public async void Create_Correct_With_Csv_Test()
    {
        // Arrage
        FormFile file = FileUtils.GetFormFile("Files/CsvTest.csv", "application/csv");
        DateTime dateTime = DateTime.Now.AddHours(1);

        NotificationDto dto = new NotificationDto()
        {
            Creator = "123",
            Title = "123",
            TimeToSend = dateTime,
            SenderEmail = "123@mail.ru",
            NotificationText = "1223",
            Body = null,
            File = file
        };

        // Act
        NotificationResponse response = await notificationService.CreateNotificationAsync(dto);

        // Assert
        Assert.Equal(ResponseStatus.Success, response.Status);
        Assert.Equal(10, response.Value.ForUsers.Count);
    }

    [Fact]
    public async void Uncorrected_Email_Test()
    {
        // Arrage
        DateTime dateTime = DateTime.Now.AddHours(1);
        NotificationDto dto = new NotificationDto()
        {
            Creator = "123",
            Title = "123",
            TimeToSend = dateTime,
            SenderEmail = "123mail.ru",
            NotificationText = "1223",
            Body = "123@mail.ru",
            File = null
        };

        // Act
        NotificationResponse response = await notificationService.CreateNotificationAsync(dto);

        // Assert
        Assert.Equal(ResponseStatus.Error, response.Status);
        Assert.Equal(localizer["WrongEmailError"], response.Message);
    }

    [Fact]
    public async void Empty_Text_And_File_Test()
    {
        // Arrage
        DateTime dateTime = DateTime.Now.AddHours(1);
        NotificationDto dto = new NotificationDto()
        {
            Creator = "123",
            Title = "123",
            TimeToSend = dateTime,
            SenderEmail = "123@mail.ru",
            NotificationText = "1223",
            Body = null,
            File = null
        };

        // Act
        NotificationResponse response = await notificationService.CreateNotificationAsync(dto);

        // Assert
        Assert.Equal(ResponseStatus.Error, response.Status);
        Assert.Equal(localizer["SendersError"], response.Message);
    }

    [Fact]
    public async void Uncorrected_Date_Test()
    {
        // Arrage
        DateTime dateTime = DateTime.Now.AddHours(-4);
        NotificationDto dto = new NotificationDto()
        {
            Creator = "123",
            Title = "123",
            TimeToSend = dateTime,
            SenderEmail = "123@mail.ru",
            NotificationText = "1223",
            Body = "123@mail.ru",
            File = null
        };

        // Act
        NotificationResponse response = await notificationService.CreateNotificationAsync(dto);

        // Assert
        Assert.Equal(ResponseStatus.Error, response.Status);
        Assert.Equal(localizer["WrongDateError"], response.Message);
    }

    [Fact]
    public async void Send_Invalid_File_Format_Test()
    {
        // Arrage
        IFormFile file = FileUtils.GetFormFile("/Files/FormatFileTest.txt", "123");
        DateTime dateTime = DateTime.Now.AddHours(1);
        NotificationDto dto = new NotificationDto()
        {
            Creator = "123",
            Title = "123",
            TimeToSend = dateTime,
            SenderEmail = "123@mail.ru",
            NotificationText = "1223",
            Body = null,
            File = file
        };

        // Act
        NotificationResponse response = await notificationService.CreateNotificationAsync(dto);

        // Assert
        Assert.Equal(ResponseStatus.Error, response.Status);
        Assert.Equal(localizer["FileFormatError"], response.Message);
    }
    
    [Fact]
    public async void File_Parse_Error_Test()
    {
        // Arrage
        IFormFile file = FileUtils.GetFormFile("/Files/FileParseErrorTest.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        DateTime dateTime = DateTime.Now.AddHours(1);
        NotificationDto dto = new NotificationDto()
        {
            Creator = "123",
            Title = "123",
            TimeToSend = dateTime,
            SenderEmail = "123@mail.ru",
            NotificationText = "1223",
            Body = null,
            File = file
        };

        // Act
        NotificationResponse response = await notificationService.CreateNotificationAsync(dto);

        // Assert
        Assert.Equal(ResponseStatus.Error, response.Status);
        Assert.Equal(localizer["FileParseError"], response.Message);
    }
    
    [Fact]
    public async void File_Parse_Specific_Table_Test()
    {
        // Arrage
        IFormFile file = FileUtils.GetFormFile("/Files/FileParseSpecificTableTest.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        DateTime dateTime = DateTime.Now.AddHours(1);
        NotificationDto dto = new NotificationDto()
        {
            Creator = "123",
            Title = "123",
            TimeToSend = dateTime,
            SenderEmail = "123@mail.ru",
            NotificationText = "1223",
            Body = null,
            File = file
        };

        // Act
        NotificationResponse response = await notificationService.CreateNotificationAsync(dto);

        // Assert
        Assert.Equal(ResponseStatus.Success, response.Status);
        Assert.Equal(20, response.Value.ForUsers.Count);
    }
}