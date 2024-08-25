using NotificationSender.Domain.Emun;
using NotificationSender.Domain.Entity;

namespace NotificationSender.Domain.Dto
{
    public class UpdateNotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime TimeToSend { get; set; }
        public List<NotificationStatus> ForUsers { get; set; }
        public string CreatorName { get; set; }
        public NotificationState NotificationState { get; set; }
    }
}
