﻿namespace NanoBroker;

public interface IConsumerOptions
{
    public string ExchangeName { get; set; }
    public string ExchangeType { get; set; }
    public string QueueName { get; set; }
    public string QueueType { get; set; }
    public string RouteName { get; set; }
    public string RouteType { get; set; }
    public bool Durable { get; set; }
    public bool Exclusive { get; set; }
    public bool AutoDelete { get; set; }

    public Dictionary<string, object> Arguments { get; set; }

    public bool AutoAcknowledgement { get; set; }
    public Action<OnRegisteredEventArgs> OnRegistered { get; set; }
    public Action<OnUnregisteredEventArgs> OnUnregistered { get; set; }
    public Action<OnConsumerCanceledEventArgs> OnConsumerCanceled { get; set; }
    public Action<OnShutdownEventArgs> OnShutdown { get; set; }
    public Action<OnReceivedEventArgs> OnReceived { get; set; }
}
