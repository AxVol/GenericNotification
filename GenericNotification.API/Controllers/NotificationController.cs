using GenericNotification.Application.Interfaces;
using GenericNotification.Domain.DTO;
using GenericNotification.Domain.Enum;
using GenericNotification.Domain.Response;
using Microsoft.AspNetCore.Mvc;

namespace GenericNotification.Controllers;

[Route("api/Notification")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationService notificationService;
    private readonly ILogger<NotificationController> logger;

    public NotificationController(INotificationService service, ILogger<NotificationController> log)
    {
        notificationService = service;
        logger = log;
    }
    
    /// <remarks>
    /// Данный эндпоинт принимает набор данных нужный для создания нотификации и списка её получателей.
    ///
    ///     POST /Notification
    ///     {
    ///         "Creator": "string", // Поле, с пометкой от чьего имени будет отправляться письмо, имя/название компании
    ///         "Title": "string", // Заголовок нотификации
    ///         "TimeToSend": "DateTime", // Время для отправки нотификации, принимается в виде -
    ///         "SenderEmail": "string", // Почта на которую будет отправляться справочная информация о состоянии нотификации
    ///         "NotificationText": "string", // Текст нотификации
    ///         "Body": "string", // Является не обязательным полем если используется File, в ином же случае ждет на ввод через запятую данные куда будет отправляться нотификация
    ///         "File": "IFile" // Является не обязательным если используется форма ввода Body. Данная форма для загрузки файла, которая ожидает на вход .cvs или .xlsx/.xls c набором данных кому отправлять нотификацию
    ///     }
    /// </remarks>
    /// <response code="200">Данные успешно отправлены</response>
    /// <response code="400">Для большей инофрмации об ошибке, смотрите описание ошибки в json файле</response>
    [HttpPost]
    public JsonResult Post(NotificationDto notification)
    {
        NotificationResponse notificationResponse = notificationService.CreateNotification(notification);

        if (notificationResponse.Status == ResponseStatus.Success)
        {
            logger.LogInformation(notificationResponse.Status.ToString());
            return new JsonResult(notificationResponse.Value);
        }
        else
        {
            logger.LogError(notificationResponse.Status.ToString());
            logger.LogInformation(notificationResponse.Message);

            return new JsonResult(notificationResponse.Message);
        }
    }
}