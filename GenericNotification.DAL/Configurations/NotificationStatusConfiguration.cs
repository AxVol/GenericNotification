using GenericNotification.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericNotification.DAL.Configurations;

public class NotificationStatusConfiguration : IEntityTypeConfiguration<NotificationStatus>
{
    public void Configure(EntityTypeBuilder<NotificationStatus> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
        builder.Property(x => x.SendStatus).IsRequired();
    }
}