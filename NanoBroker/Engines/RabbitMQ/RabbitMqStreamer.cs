using RabbitMQ.Client;

namespace NanoBroker.Engines.RabbitMQ;

public class RabbitMqStreamer : IStreamer
{
    public IStreamerOptions Options { get; }

    private RabbitMqBroker _client;
    private IBasicProperties _basicProperties;

    internal RabbitMqStreamer(RabbitMqBroker client, IStreamerOptions options)
    {
        _client = client;
        Options = options;
    }

    public void Connect() => _client.Connect();
    public void Disconnect() => _client.Disconnect();

    public void Stream(byte[] data) => Stream(data.AsMemory());
    public void Stream(string text) => Stream(Encoding.UTF8.GetBytes(text));

    public void Stream(ReadOnlyMemory<byte> data)
    {
        // Basic Properties
        if (_basicProperties == null)
            _basicProperties = _client.Session.CreateBasicProperties();

        // Stream
        _client.Session.BasicPublish(Options.ExchangeName, Options.RoutingKey, false, _basicProperties, data);
    }

}
