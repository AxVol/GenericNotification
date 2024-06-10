using GenericNotification.Application.Interfaces;
using GenericNotification.Application.Resources;
using GenericNotification.Application.Service;
using GenericNotification.DAL.Repository;
using GenericNotification.Domain.DTO;
using GenericNotification.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Xunit;

namespace GenericNotification.Tests;

public class NotificationServiceTests
{
    private readonly INotificationService notificationService;

    public NotificationServiceTests()
    {
        ILogger<NotificationService> logger = Mock.MockFactory<NotificationService>.GetLogger();
        IRepository<Notification> repository = Mock.MockFactory<NotificationService>.GetNotificationRepository();
        IParser parser = Mock.MockFactory<NotificationService>.GetParser();
        IStringLocalizer<Resources> localizer = Mock.MockFactory<NotificationService>.GetLocalaizer();

        notificationService = new NotificationService(localizer, parser, logger, repository);
    }

    [Fact]
    public async void Create_Currect_With_Body_Text_Test()
    {
        // Arrage


        IFormFile file = new FormFile()
        {

        };
        
        NotificationDto dto = new NotificationDto()
        {
            
        };

        // Act
        //

        // Assert
        //
    }

    public async void Create_Currect_With_Excel_Test()
    {
        
    }

    public async void Create_Currect_With_Csv_Test()
    {
        
    }

    public async void Uncorrect_Email_Test()
    {
        
    }

    public async void Empty_Text_And_File_Test()
    {
        
    }

    public async void Uncorrect_Date_Test()
    {
        
    }
    
    
}