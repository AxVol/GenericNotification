using Microsoft.AspNetCore.Mvc;
using NotificationSender.Application.Interfaces;
using NotificationSender.Domain.Dto;
using NotificationSender.Domain.Emun;
using NotificationSender.Domain.Response;

namespace NotificationSender.Controllers;

[Route("api/SendNotification")]
[ApiController]
public class NotificationSenderController : ControllerBase
{
    private readonly INotificationService notificationService;
    
    public NotificationSenderController(INotificationService service)
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

    [HttpGet]
    public async Task<IActionResult> GetProcessedUsers(NotificationDto notification)
    {
        UsersProcessedResponse notificationResponse = await notificationService.GetProcessedUsersAsync(notification);

        if (notificationResponse.Status == ResponseStatus.Success)
        {
            return new JsonResult(notificationResponse.Value.ToString());
        }

        return new JsonResult(notificationResponse.Message);
    }
}