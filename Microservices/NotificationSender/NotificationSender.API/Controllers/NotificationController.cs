using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NotificationSender.Application.Interfaces;
using NotificationSender.Domain.Dto;
using NotificationSender.Domain.Emun;
using NotificationSender.Domain.Response;

namespace NotificationSender.Controllers;

[Route("api/Notification")]
[ApiController]
public class NotificationController
{
    private readonly INotificationService notificationService;
    
    public NotificationController(INotificationService service)
    {
        notificationService = service;
    }
    
    /// <remarks>
    /// Данный эндпоинт принимает id нотификации в виде uuid и возвращает данные о запрошеной нотификации
    ///
    ///     GET /Notification
    ///     {
    ///         "id": uuid
    ///     }
    ///
    ///      string - Статус нотификации
    ///      (
    ///          InProgress - Нотификация в обработке 
    ///          NotStarted - Время отправки ещё не наступило,
    ///          Finished - Нотификация обработана
    ///      )
    /// </remarks>
    /// <response code="200">Данные успешно отправлены</response>
    /// <response code="400">Для большей инофрмации об ошибке, смотрите описание ошибки в json файле</response>
    [HttpGet]
    public async Task<IActionResult> GetNotificationState(Guid id, DateTime publishDate)
    {
        NotificationDto notification = new NotificationDto(){ Id = id, PublishDate = publishDate };
        NotificationStateResponse notificationState = await notificationService.GetNotificationStateAsync(notification);

        if (notificationState.Status == ResponseStatus.Success)
        {
            return new JsonResult(notificationState.Value);
        }

        return new JsonResult(notificationState.Message);
    }
}