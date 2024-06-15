using GenericNotification.DAL.Repository;
using GenericNotification.DAL.Repository.Implementations;
using GenericNotification.DAL.Repository.Interfaces;
using GenericNotification.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace GenericNotification.DAL.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("postgresql");

        serviceCollection.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.EnableSensitiveDataLogging();
        });
        serviceCollection.AddScoped<IRepository<Notification>, NotificationRepository>();
        serviceCollection.AddScoped<IDbRepository<NotificationStatus>, NotificationStatusRepository>();

        return serviceCollection;
    }
}