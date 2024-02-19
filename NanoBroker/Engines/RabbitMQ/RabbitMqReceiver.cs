using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NanoBroker.Engines.RabbitMQ;

public class RabbitMqReceiver : IReceiver
{
    public IReceiverOptions Options { get; }

    private string _tag;
    private RabbitMqBroker _client;
    private EventingBasicConsumer _consumer;

    internal RabbitMqReceiver(RabbitMqBroker client, IReceiverOptions options)
    {
        _client = client;
        Options = options;

        _consumer = new EventingBasicConsumer(_client.Session);
        _consumer.ConsumerCancelled += (ch, ea) => options.OnConsumerCanceled?.Invoke(new OnCanceledEventArgs());
        _consumer.Registered += (ch, ea) => options.OnRegistered?.Invoke(new OnRegisteredEventArgs());
        _consumer.Unregistered += (ch, ea) => options.OnUnregistered?.Invoke(new OnUnregisteredEventArgs());
        _consumer.Shutdown += (ch, ea) => options.OnShutdown?.Invoke(new OnShutdownEventArgs());
        _consumer.Received += (ch, ea) =>
        {
            _client.Session.BasicAck(ea.DeliveryTag, true);
            options.OnReceived?.Invoke(new OnReceivedEventArgs { Data = ea.Body.ToArray() });
        };
    }

    public void Connect() => _client.Connect();
    public void Disconnect() => _client.Disconnect();

    public void Start()
    {
        _client.Connect();
        _client.Session.BasicQos((uint)Options.QosPrefetchSize, (ushort)Options.QosPrefetchCount, Options.QosGlobal); // PrefetchCount is required
        _tag = _client.Session.BasicConsume(Options.QueueName, false, _consumer);
    }

    public void Stop()
    {
        _client.Session.BasicCancel(_tag);
    }
}
