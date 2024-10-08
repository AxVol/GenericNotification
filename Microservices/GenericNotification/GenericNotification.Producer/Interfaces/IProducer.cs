﻿namespace GenericNotification.Producer.Interfaces;

public interface IProducer : IDisposable
{
    Task Publish<T>(T obj, string routingKey, string ttl = "30000");
}