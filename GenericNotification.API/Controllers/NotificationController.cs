using GenericNotification.Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GenericNotification.Controllers;

[Route("api/Notification")]
[ApiController]
public class NotificationController : ControllerBase
{
    [HttpPost]
    public JsonResult Post(NotificationDto notification)
    {
        return new JsonResult("");
    }
}