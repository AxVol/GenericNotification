using GenericNotification.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericNotification.Application.Service;

public class UpdateNotificationListHostedService : BackgroundService
{
    private readonly ILogger<UpdateNotificationListHostedService> logger;
    private readonly INotificationService notificationService;
    private Timer? timer = null;
    private readonly int refreshInterval = 5; // время в минутах

    public UpdateNotificationListHostedService(ILogger<UpdateNotificationListHostedService> log,
        INotificationService service)
    {
        logger = log;
        notificationService = service;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(refreshInterval));
        
        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        
    }
}