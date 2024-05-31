using Microsoft.AspNetCore.Mvc;

namespace GenericNotification.Controllers;

[Route("Notification")]
public class NotificationController : ControllerBase
{
    public JsonResult Get()
    {
        return new JsonResult("");
    }
    
    public JsonResult Post()
    {
        return new JsonResult("");
    }
    
    public JsonResult Put()
    {
        return new JsonResult("");
    }
    
    public JsonResult Delete()
    {
        return new JsonResult("");
    }
}