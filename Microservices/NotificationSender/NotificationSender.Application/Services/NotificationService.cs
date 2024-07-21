using Microsoft.Extensions.Logging;
using NotificationSender.Application.Interfaces;
using NotificationSender.DAL.Repository.Interfaces;
using NotificationSender.Domain.Dto;
using NotificationSender.Domain.Emun;
using NotificationSender.Domain.Entity;
using NotificationSender.Domain.Interfaces;
using NotificationSender.Domain.Response;

namespace NotificationSender.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository repository;
    private readonly ILogger<NotificationService> logger;

    public NotificationService(INotificationRepository rep, ILogger<NotificationService> log)
    {
        repository = rep;
        logger = log;
    }
    
    public async Task<NotificationStateResponse> GetNotificationStateAsync(NotificationDto notificationDto)
    {
        NotificationStateResponse notificationResponse = new NotificationStateResponse();
        Notification notification = new Notification();
        notification.Id = notificationDto.Id;
        notification.TimeToSend = notificationDto.PublishDate;

        try
        {
            notification = await repository.GetAsync(notification);
        }
        catch (InvalidDataException ex)
        {
            logger.LogInformation(ex.Message);
            notificationResponse.Status = ResponseStatus.Error;
            notificationResponse.Message = ex.Message;

            return notificationResponse;
        }

        notificationResponse.Status = ResponseStatus.Success;
        notificationResponse.Value = notification.NotificationState.ToString();

        return notificationResponse;
    }

    public async Task<UsersProcessedResponse> GetProcessedUsersAsync(NotificationDto notificationDto)
    {
        UsersProcessedResponse notificationResponse = new UsersProcessedResponse();
        Notification notification = new Notification();
        notification.Id = notificationDto.Id;
        notification.TimeToSend = notificationDto.PublishDate;

        try
        {
            notification = await repository.GetAsync(notification);
        }
        catch (InvalidDataException ex)
        {
            logger.LogInformation(ex.Message);
            notificationResponse.Status = ResponseStatus.Error;
            notificationResponse.Message = ex.Message;

            return notificationResponse;
        }

        notificationResponse.Status = ResponseStatus.Success;
        notificationResponse.Value = notification.CountNotifications - notification.ForUsers.Count;

        return notificationResponse;
    }
}