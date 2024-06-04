using GenericNotification.Application.Interfaces;
using GenericNotification.Domain.DTO;
using GenericNotification.Domain.Enum;
using GenericNotification.Domain.Response;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace GenericNotification.Application.Service;

public class NotificationService : INotificationService
{
    private readonly IStringLocalizer<Resources.Resources> localizationMessages;

    public NotificationService(IStringLocalizer<Resources.Resources> localizer)
    {
        localizationMessages = localizer;
    }
    
    public NotificationResponse CreateNotification(NotificationDto notificationDto)
    {
        NotificationResponse notificationResponse = NotificationValidate(notificationDto);
        
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
                if (notificationDto.File.ContentType == "application/vnd.ms-excel" 
                    || notificationDto.File.ContentType == "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    || notificationDto.File.ContentType == "text/csv"
                    || notificationDto.File.ContentType == "application/csv")
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