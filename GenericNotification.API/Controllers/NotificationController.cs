using GenericNotification.Application.Resources;
using GenericNotification.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GenericNotification.Controllers;

[Route("api/Notification")]
[ApiController]
public class NotificationController : ControllerBase
{
    public NotificationController()
    {
        
    }
    
    [HttpPost]
    public JsonResult Post(NotificationDto notification)
    {
        return new JsonResult("");
    }
}