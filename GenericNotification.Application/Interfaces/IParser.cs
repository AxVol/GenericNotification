﻿using GenericNotification.Domain.Entity;
using Microsoft.AspNetCore.Http;

namespace GenericNotification.Application.Interfaces;

public interface IParser
{
    public Dictionary<string, string> FileExtensions { get; }

    public Task<Queue<NotificationStatus>> ParseAsync(IFormFile file);
    public Task<Queue<NotificationStatus>> ParseAsync(string text);
}