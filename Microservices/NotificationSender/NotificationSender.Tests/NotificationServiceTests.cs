using Microsoft.Extensions.Logging;
using NotificationSender.Application.Interfaces;
using NotificationSender.Application.Services;
using NotificationSender.DAL.Repository.Interfaces;
using NotificationSender.Domain.Dto;
using NotificationSender.Domain.Emun;
using NotificationSender.Domain.Response;
using Xunit;

namespace NotificationSender.Tests;

public class NotificationServiceTests
{
    private readonly INotificationService notificationService;

    public NotificationServiceTests()
    {
        INotificationRepository repository = Mock.MockFactory<NotificationService>.GetNotificationRepository();
        ILogger<NotificationService> logger = Mock.MockFactory<NotificationService>.GetLogger();
        notificationService = new NotificationService(repository, logger);
    }

    [Fact]
    public async Task Get_Exists_Notification_State_Test()
    {
        // Arrage
        Guid guid;
        bool isGuid = Guid.TryParse("4dfc6b14-7213-3363-8009-b23c56e3a1b1", out guid);
        NotificationDto dto = new NotificationDto();
        dto.Id = guid;
        dto.PublishDate = DateTime.Now;
        
        // Act
        NotificationStateResponse response = await notificationService.GetNotificationStateAsync(dto);

        // Assert
        Assert.Equal(ResponseStatus.Success, response.Status);
        Assert.Equal("NotStarted", response.Value);
    }

    [Fact]
    public async Task Get_Processed_Users_Test()
    {
        // Arrage
        Guid guid;
        bool isGuid = Guid.TryParse("4dfc6b14-7213-3363-8009-b23c56e3a1b1", out guid);
        NotificationDto dto = new NotificationDto();
        dto.Id = guid;
        dto.PublishDate = DateTime.Now;
        
        // Act
        UsersProcessedResponse response = await notificationService.GetProcessedUsersAsync(dto);

        // Assert
        Assert.Equal(2, response.Value);
        Assert.Equal(ResponseStatus.Success, response.Status);
    }
}