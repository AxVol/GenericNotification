using GenericNotification.Application.Interfaces;
using GenericNotification.Domain.DTO;
using GenericNotification.Domain.Enum;
using GenericNotification.Domain.Response;
using Microsoft.AspNetCore.Http.HttpResults;
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
    ///         "TimeToSend": "DateTime", // Время для отправки нотификации, принимается в виде - MMDDYYYY H:M
    ///         "SenderEmail": "string", // Почта на которую будет отправляться справочная информация о состоянии нотификации
    ///         "NotificationText": "string", // Текст нотификации
    ///         "Body": "string", // Является не обязательным полем если используется File, в ином же случае ждет на ввод через запятую данные куда будет отправляться нотификация
    ///         "File": "IFile" // Является не обязательным если используется форма ввода Body. Данная форма для загрузки файла, которая ожидает на вход .cvs или .xlsx/.xls c набором данных кому отправлять нотификацию
    ///     }
    /// </remarks>
    /// <response code="200">Данные успешно отправлены</response>
    /// <response code="400">Для большей инофрмации об ошибке, смотрите описание ошибки в json файле</response>
    [HttpPost]
    public async Task<JsonResult> Post(NotificationDto notification)
    {
        notification.TimeToSend = notification.TimeToSend.ToUniversalTime();
        NotificationResponse notificationResponse = await notificationService.CreateNotificationAsync(notification);

        if (notificationResponse.Status == ResponseStatus.Error)
        {
            logger.LogError(notificationResponse.Status.ToString());
            logger.LogInformation(notificationResponse.Message);

            return new JsonResult(notificationResponse.Message);
        }

        await notificationService.SendNotificationAsync(notificationResponse.Value);

        return new JsonResult(notificationResponse.Status.ToString());
    }

    /// <remarks>
    /// Данный эндпоинт принимает id нотификации в виде uuid и возвращает данные о запрошеной нотификации
    ///
    ///     GET /Notification
    ///     {
    ///         "id": uuid
    ///     }
    ///
    ///      Notification
    ///     {
    ///         "id": uuid,
    ///         "Title": "string", // Название нотификации
    ///         "Body": "string", // Содержание нотификации
    ///         "TimeToSend": DateTime, // Время запланированной публикации
    ///         "IsSend": bool, // Статус нотификации, True - Отправлена полностью. False - Не отправлена
    ///         "ForUsers" : List
    ///         {
    ///              "id": uuid,
    ///              "Email": "string", // Адрес для отправки нотификации
    ///              "SendStatus": bool // Статус нотификации для конкретного пользователя
    ///         }
    ///         "CreatorName": "string" Создатель нотификации
    ///     }
    /// </remarks>
    /// <response code="200">Данные успешно отправлены</response>
    /// <response code="400">Для большей инофрмации об ошибке, смотрите описание ошибки в json файле</response>
    [HttpGet]
    public async Task<JsonResult> Get(Guid id)
    {
        NotificationResponse notification = await notificationService.GetNotificationAsync(id);

        if (notification.Status == ResponseStatus.Error)
        {
            return new JsonResult(notification.Message);
        }

        return new JsonResult(notification);
    }

    /// <remarks>
    /// Данный эндпоинт принимает нотификацию для её удаления из базы данных
    ///
    ///     DELETE /Notification
    ///     {
    ///         "id": uuid
    ///     }
    /// </remarks>
    /// <response code="200">Данные успешно удалены</response>
    /// <response code="400">Для большей инофрмации об ошибке, смотрите описание ошибки в json файле</response>
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        NotificationResponse notification = await notificationService.DeleteNotificationAsync(id);

        if (notification.Status == ResponseStatus.Error)
        {
            return new JsonResult(notification.Message);
        }

        return Ok();
    }
}