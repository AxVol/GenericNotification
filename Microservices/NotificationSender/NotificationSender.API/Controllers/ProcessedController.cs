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