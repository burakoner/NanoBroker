using RabbitMQ.Client;

namespace NanoBroker.Engines.RabbitMQ;

public class RabbitMqPublisher : IPublisher
{
    public IPublisherOptions Options { get; }

    private RabbitMqBroker _client;
    private IBasicProperties _basicProperties;

    internal RabbitMqPublisher(RabbitMqBroker client, IPublisherOptions options)
    {
        _client = client;
        Options = options;
    }

    public void Connect() => _client.Connect();
    public void Disconnect() => _client.Disconnect();

    public void Publish(byte[] data) => Publish(data.AsMemory());
    public void Publish(string data) => Publish(Encoding.UTF8.GetBytes(data));

    public void Publish(ReadOnlyMemory<byte> data)
    {
        // Basic Properties
        if (_basicProperties == null)
            _basicProperties = _client.Session.CreateBasicProperties();

        // Publish
        _client.Session.BasicPublish(Options.ExchangeName, Options.RouteName, false, _basicProperties, data);
    }

}
