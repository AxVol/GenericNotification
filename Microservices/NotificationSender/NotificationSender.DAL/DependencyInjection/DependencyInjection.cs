using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationSender.DAL.Repository.Implementations;
using NotificationSender.DAL.Repository.Interfaces;
using NotificationSender.Domain.Entity;

namespace NotificationSender.DAL.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLevel(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        string redisConf = configuration.GetConnectionString("RedisConnection");
        string instance = configuration["RedisInstance"];
        
        serviceCollection.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = redisConf;
            opt.InstanceName = instance;
        });
        serviceCollection.AddTransient<INotificationRepository, NotificationRepository>();

        return serviceCollection;
    }
}