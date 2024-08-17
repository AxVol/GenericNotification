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
    
    [HttpGet]
    public async Task<IActionResult> GetNotificationState(NotificationDto notification)
    {
        NotificationStateResponse notificationState = await notificationService.GetNotificationStateAsync(notification);

        if (notificationState.Status == ResponseStatus.Success)
        {
            return new JsonResult(notificationState.Value);
        }

        return new JsonResult(notificationState.Message);
    }
}