﻿using System.Text;
using GenericNotification.Producer.Interfaces;
using RabbitMQ.Client;

namespace GenericNotification.Producer.Implementations;

public class Producer : IProducer
{
    private readonly IConnectionProvider connectionProvider;
    private readonly string exchange;
    private readonly IModel model;

    public Producer(IConnectionProvider conProvider, string ex)
    {
        connectionProvider = conProvider;
        exchange = ex;
        model = connectionProvider.GetConnection().CreateModel();
    }
    
    public async Task Publish(string message, string routingKey)
    {
        await Task.Run(() =>
        {
            byte[] body = Encoding.UTF8.GetBytes(message);
            IBasicProperties properties = model.CreateBasicProperties();
        
            model.BasicPublish(exchange, routingKey, properties, body);
        });
    }
    
    public void Dispose()
    {
        model.Close();
        GC.SuppressFinalize(this);
    }
}