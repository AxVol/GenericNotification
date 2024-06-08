﻿using GenericNotification.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericNotification.DAL.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Title).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Body).IsRequired().HasMaxLength(300);
        builder.Property(x => x.CreatorName).IsRequired();
        builder.Property(x => x.TimeToSend).IsRequired();
        builder.Property(x => x.IsSend).IsRequired();
        builder.HasMany<NotificationStatus>(x => x.ForUsers);

    }
}