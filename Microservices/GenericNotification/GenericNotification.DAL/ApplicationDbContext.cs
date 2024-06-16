using System.Reflection;
using GenericNotification.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace GenericNotification.DAL;

public class ApplicationDbContext : DbContext
{
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationStatus> NotificationStatus { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions) : base(contextOptions)
    {
        Database.EnsureCreated(); 
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}