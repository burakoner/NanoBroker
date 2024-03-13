using RabbitMQ.Client;

namespace NanoBroker.Engines.RabbitMQ;

public class RabbitMqStreamer : IStreamer
{
    public IStreamerOptions Options { get; }

    private readonly RabbitMqBroker _client;
    private IBasicProperties _basicProperties;

    internal RabbitMqStreamer(RabbitMqBroker client, IStreamerOptions options)
    {
        _client = client;
        Options = options;
    }

    public void Connect() => _client.Connect();
    public void Disconnect() => _client.Disconnect();

    public void Stream(byte[] data) => Stream(data.AsMemory());
    public void Stream(string data) => Stream(Encoding.UTF8.GetBytes(data));
    
    public async Task StreamAsync(byte[] data, CancellationToken ct = default)
    {
        Stream(data);
        await Task.CompletedTask;
    }
    public async Task StreamAsync(string data, CancellationToken ct = default)
    {
        Stream(data);
        await Task.CompletedTask;
    }

    private void Stream(ReadOnlyMemory<byte> data)
    {
        // Basic Properties
        if (_basicProperties is null)
            _basicProperties = _client.Session.CreateBasicProperties();

        // Stream
        _client.Session.BasicPublish(Options.ExchangeName, Options.RouteName, false, _basicProperties, data);
    }

}
