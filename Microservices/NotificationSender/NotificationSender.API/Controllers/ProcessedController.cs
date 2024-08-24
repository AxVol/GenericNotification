using Microsoft.AspNetCore.Mvc;
using NotificationSender.Application.Interfaces;
using NotificationSender.Domain.Dto;
using NotificationSender.Domain.Emun;
using NotificationSender.Domain.Response;

namespace NotificationSender.Controllers;

[Route("api/Processed")]
[ApiController]
public class ProcessedController
{
    private readonly INotificationService notificationService;
    
    public ProcessedController(INotificationService service)
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
    ///      int - Количество пользваталей которые получили нотификацию
    /// </remarks>
    /// <response code="200">Данные успешно отправлены</response>
    /// <response code="400">Для большей инофрмации об ошибке, смотрите описание ошибки в json файле</response>
    [HttpGet]
    public async Task<IActionResult> GetProcessedUsers(Guid id, DateTime publishDate)
    {
        NotificationDto notification = new NotificationDto(){ Id = id, PublishDate = publishDate.ToUniversalTime() };
        UsersProcessedResponse notificationResponse = await notificationService.GetProcessedUsersAsync(notification);
    
        if (notificationResponse.Status == ResponseStatus.Success)
        {
            return new JsonResult(notificationResponse.Value.ToString());
        }

        return new JsonResult(notificationResponse.Message);
    }
}