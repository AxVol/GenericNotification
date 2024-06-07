using GenericNotification.Application.Interfaces;
using GenericNotification.Domain.DTO;
using GenericNotification.Domain.Enum;
using GenericNotification.Domain.Response;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using GenericNotification.Domain.Entity;
using Microsoft.Extensions.Logging;

namespace GenericNotification.Application.Service;

public class NotificationService : INotificationService
{
    private readonly IStringLocalizer<Resources.Resources> localizationMessages;
    private readonly IParser parserService;
    private readonly ILogger<NotificationService> logger;

    public NotificationService(IStringLocalizer<Resources.Resources> localizer, IParser parser, 
        ILogger<NotificationService> log)
    {
        localizationMessages = localizer;
        parserService = parser;
        logger = log;
    }
    
    public NotificationResponse CreateNotification(NotificationDto notificationDto)
    {
        NotificationResponse notificationResponse = NotificationValidate(notificationDto);
        
        if (notificationResponse.Status == ResponseStatus.Error)
        {
            return notificationResponse;
        }
        
        Queue<NotificationStatus> notificationStatus;
        Notification notification = new Notification()
        {
            Title = notificationDto.Title,
            TimeToSend = notificationDto.TimeToSend,
            Body = notificationDto.NotificationText,
            IsSend = false,
            CreatorName = notificationDto.SenderEmail
        };

        try
        {
            if (notificationDto.Body == null || (notificationDto.Body != null && notificationDto.File != null))
            { 
                notificationStatus = parserService.Parse(notificationDto.File, notification);
            }
            else
            {
                notificationStatus = parserService.Parse(notificationDto.Body, notification);
            }
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex.Message);

            notificationResponse.Status = ResponseStatus.Error;
            notificationResponse.Message = ex.Message;

            return notificationResponse;
        }

        notification.ForUsers = notificationStatus;
        notificationResponse.Value = notification;

        return notificationResponse;
    }

    private NotificationResponse NotificationValidate(NotificationDto notificationDto)
    {
        bool isEmail = new EmailAddressAttribute().IsValid(notificationDto.SenderEmail);
        NotificationResponse notificationResponse = new NotificationResponse();
        notificationDto.TimeToSend =  notificationDto.TimeToSend.ToUniversalTime();

        if (notificationDto.TimeToSend < DateTime.UtcNow)
        {
            notificationResponse.Message = localizationMessages["WrongDateError"];
            notificationResponse.Status = ResponseStatus.Error;

            return notificationResponse;
        }
        
        if (!isEmail)
        {
            notificationResponse.Message = localizationMessages["WrongEmailError"];
            notificationResponse.Status = ResponseStatus.Error;

            return notificationResponse;
        }

        if (notificationDto.Body == null && notificationDto.File == null)
        {
            notificationResponse.Message = localizationMessages["SendersError"];
            notificationResponse.Status = ResponseStatus.Error;

            return notificationResponse;
        }
        else
        {
            if (notificationDto.Body != null)
            {
                notificationResponse.Status = ResponseStatus.Success;

                return notificationResponse;
            }
            else
            {
                if (parserService.FileExtensions.ContainsKey(notificationDto.File.ContentType))
                {
                    notificationResponse.Status = ResponseStatus.Success;

                    return notificationResponse;
                }
                else
                {
                    notificationResponse.Message = localizationMessages["FileFormatError"];
                    notificationResponse.Status = ResponseStatus.Error;

                    return notificationResponse;
                }
            }
        }
    }
}