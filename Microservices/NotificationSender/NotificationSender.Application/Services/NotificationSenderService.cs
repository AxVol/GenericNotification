using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using MimeKit;
using NotificationSender.Application.Interfaces;
using NotificationSender.DAL.Repository.Interfaces;
using NotificationSender.Domain.Emun;
using NotificationSender.Domain.Entity;
using NotificationSender.Domain.Resources;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace NotificationSender.Application.Services;

public class NotificationSenderService : INotificationSenderService
{
    private const int mailPort = 25;
    
    private readonly ILogger<NotificationSenderService> logger;
    private readonly IStringLocalizer<Resources> localizer;
    private readonly INotificationRepository repository;
    
    private readonly string appMail;
    private readonly string mailHost;
    private readonly string password;
    private readonly string appName;
    
    public NotificationSenderService(ILogger<NotificationSenderService> log, IStringLocalizer<Resources> localize,
        IConfiguration config, INotificationRepository rep)
    {
        logger = log;
        localizer = localize;
        appMail = config["AppMail"];
        mailHost = config["MailHost"];
        password = config["Password"];
        repository = rep;
    }
    
    public async Task SendNotificationAsync(Notification notification)
    {
        logger.LogInformation($"Start sending notification {notification.Id}");
        notification.NotificationState = NotificationState.InProgress;

        using (SmtpClient client = new SmtpClient())
        {
            await client.ConnectAsync(mailHost, mailPort, false);
            await client.AuthenticateAsync(appMail, password);

            using (MimeMessage emailMessage = new MimeMessage())
            {
                emailMessage.From.Add(new MailboxAddress(notification.CreatorName, appMail));
                emailMessage.Subject = notification.Title;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = notification.Body
                };

                foreach (NotificationStatus notificationForSend in notification.ForUsers)
                {
                    logger.LogInformation($"Sending notification to user with id {notificationForSend.Id}");
                    
                    emailMessage.To.Add(new MailboxAddress("", notificationForSend.Email));

                    try
                    {
                        await client.SendAsync(emailMessage);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message);
                        throw new SmtpException(localizer["ErrorSendMessage"]);
                    }

                    logger.LogInformation($"Notification is Sended id - {notificationForSend.Id}");
                }
            }
            await client.DisconnectAsync(true);
        }
    }
}