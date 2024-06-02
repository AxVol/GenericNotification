using GenericNotification.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericNotification.DAL.Configurations;

public class NotificationStatusConfiguration : IEntityTypeConfiguration<NotificationStatus>
{
    public void Configure(EntityTypeBuilder<NotificationStatus> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(12);
        builder.Property(x => x.SendStatus).IsRequired();
        builder.HasOne<Notification>(x => x.Notification)
            .WithMany(x => x.ForUsers)
            .HasForeignKey(x => x.NotificationId);
    }
}