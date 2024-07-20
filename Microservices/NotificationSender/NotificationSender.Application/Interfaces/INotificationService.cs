using NotificationSender.Domain.Dto;
using NotificationSender.Domain.Response;

namespace NotificationSender.Application.Interfaces;

public interface INotificationService
{
    public Task<NotificationStateResponse> GetNotificationStateAsync(NotificationDto notificationDto);
    public Task<UsersProcessedResponse> GetProcessedUsersAsync(NotificationDto notificationDto);
}