using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GenericNotification.Domain.DTO;

public class NotificationDto
{
    [Required]
    public string Creator { get; set; }
    [Required] 
    public string Title { get; set; }
    [Required]
    public DateTime TimeToSend { get; set; }
    [Required]
    public string SenderEmail { get; set; }
    public string Body { get; set; }
    public IFormFile File { get; set; }
}