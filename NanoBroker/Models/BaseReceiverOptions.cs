namespace NanoBroker;

public class BaseReceiverOptions : IReceiverOptions
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

    public bool QosGlobal { get; set; } = false;
    public int QosPrefetchSize { get; set; } = 0;
    public int QosPrefetchCount { get; set; } = 100;

    public Action<OnRegisteredEventArgs> OnRegistered { get; set; }
    public Action<OnUnregisteredEventArgs> OnUnregistered { get; set; }
    public Action<OnCanceledEventArgs> OnConsumerCanceled { get; set; }
    public Action<OnShutdownEventArgs> OnShutdown { get; set; }
    public Action<OnReceivedEventArgs> OnReceived { get; set; }
}
