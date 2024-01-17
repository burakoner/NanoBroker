namespace NanoBroker;

public class BasePublisherOptions : IPublisherOptions
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

    public IDictionary<string, object> Arguments { get; set; }
}
