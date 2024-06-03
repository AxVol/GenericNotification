using GenericNotification.DAL.Repository;
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
        serviceCollection.AddScoped<IRepository<NotificationStatus>, NotificationStatusRepository>();

        return serviceCollection;
    }
}