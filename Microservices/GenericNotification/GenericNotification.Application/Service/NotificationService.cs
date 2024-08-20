using GenericNotification.Application.Interfaces;
using GenericNotification.Domain.DTO;
using GenericNotification.Domain.Enum;
using GenericNotification.Domain.Response;
using GenericNotification.Domain.Resources;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using GenericNotification.DAL.Repository.Interfaces;
using GenericNotification.Domain.Entity;
using Microsoft.Extensions.Logging;

namespace GenericNotification.Application.Service;

public class NotificationService : INotificationService
{
    private const string BrokerRoutingKey = "NotificationSend";
    
    private readonly IStringLocalizer<Resources> localizationMessages;
    private readonly IParser parserService;
    private readonly ILogger<NotificationService> logger;
    private readonly IRepository<Notification> repository;

    public NotificationService(IStringLocalizer<Resources> localizer, IParser parser, 
        ILogger<NotificationService> log, IRepository<Notification> rep)
    {
        localizationMessages = localizer;
        parserService = parser;
        logger = log;
        repository = rep;
    }

    public async Task<NotificationResponse> GetNotificationAsync(Guid id)
    {
        NotificationResponse notificationResponse = await Task.Run(() =>
        {
            NotificationResponse notificationResponse = new NotificationResponse();
            Notification? notification = repository.GetAll().FirstOrDefault(n => n.Id == id);

            if (notification is null)
            {
                notificationResponse.Status = ResponseStatus.Error;
                notificationResponse.Message = localizationMessages["NotFoundError"];

                return notificationResponse;
            }

            notificationResponse.Status = ResponseStatus.Success;
            notificationResponse.Value = notification;
            
            return notificationResponse;
        });

        return notificationResponse;
    }

    public async Task SendNotificationAsync(Notification notification)
    {
        if (IsToday(notification.TimeToSend))
            await repository.AddToBrokerAsync(notification, BrokerRoutingKey);
    }
    
    public async Task<NotificationResponse> CreateNotificationAsync(NotificationDto notificationDto)
    {
        List<NotificationStatus> notificationStatus;
        
        notificationDto.TimeToSend = notificationDto.TimeToSend.ToUniversalTime();
        NotificationResponse notificationResponse = NotificationValidate(notificationDto);
           
        if (notificationResponse.Status == ResponseStatus.Error)
        {
            return notificationResponse;
        }
        
        Notification notification = new Notification()
        {
            Id = Guid.NewGuid(),
            Title = notificationDto.Title,
            TimeToSend = notificationDto.TimeToSend,
            Body = notificationDto.NotificationText,
            IsSend = false,
            CreatorName = notificationDto.SenderEmail
        };

        try
        {
            if (notificationDto.TextReceivers == null)
            {
                notificationStatus = parserService.Parse(notificationDto.File);
            }
            else
            {
                notificationStatus = parserService.Parse(notificationDto.TextReceivers);
            }
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex.Message);

            notificationResponse.Status = ResponseStatus.Error;
            notificationResponse.Message = ex.Message;

            return notificationResponse;
        }
        catch (NullReferenceException ex)
        {
            logger.LogError(ex.Message);

            notificationResponse.Status = ResponseStatus.Error;
            notificationResponse.Message = localizationMessages["FileParseError"];

            return notificationResponse;
        }

        notification.ForUsers = notificationStatus;
        notificationResponse.Value = notification;

        await repository.CreateAsync(notification);
        
        return notificationResponse;
    }

    private NotificationResponse NotificationValidate(NotificationDto notificationDto)
    {
        bool isEmail = new EmailAddressAttribute().IsValid(notificationDto.SenderEmail);
        NotificationResponse notificationResponse = new NotificationResponse();

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

        if (notificationDto.TextReceivers == null && notificationDto.File == null)
        {
            notificationResponse.Message = localizationMessages["SendersError"];
            notificationResponse.Status = ResponseStatus.Error;

            return notificationResponse;
        }

        if (notificationDto.TextReceivers != null && notificationDto.File != null)
        {
            notificationResponse.Message = localizationMessages["FileAndReceiverError"];
            notificationResponse.Status = ResponseStatus.Error;

            return notificationResponse;
        }
        
        if (notificationDto.TextReceivers != null)
        {
            notificationResponse.Status = ResponseStatus.Success;

            return notificationResponse;
        }
        
        if (!parserService.FileExtensions.ContainsKey(notificationDto.File.ContentType))
        {
            notificationResponse.Message = localizationMessages["FileFormatError"];
            notificationResponse.Status = ResponseStatus.Error;

            return notificationResponse;
        }
        
        notificationResponse.Status = ResponseStatus.Success;

        return notificationResponse;
    }

    public async Task<NotificationResponse> DeleteNotificationAsync(Guid id)
    {
        NotificationResponse notificationResponse = new NotificationResponse();
        Notification? notification = repository.GetAll().FirstOrDefault(n => n.Id == id);

        if (notification is null)
        {
            notificationResponse.Status = ResponseStatus.Error;
            notificationResponse.Message = localizationMessages["NotFoundError"];

            return notificationResponse;
        }

        notificationResponse.Status = ResponseStatus.Success;
        notificationResponse.Value = notification;

        await repository.DeleteAsync(notification);
            
        return notificationResponse;
    }

    private bool IsToday(DateTime notificationDate)
    {
        DateTime currentDate = DateTime.UtcNow;
        bool isCurrentDay = currentDate.Day == notificationDate.Day;
        bool isCurrentMonth = currentDate.Month == notificationDate.Month;
        bool isCurrentYear = currentDate.Year == notificationDate.Year;

        if (isCurrentDay && isCurrentMonth && isCurrentYear)
        {
            return true;
        }

        return false;
    }
}