using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NanoBroker.Engines.RabbitMQ;

public class RabbitMqRpcServer : IRpcServer
{
    public IRpcServerOptions Options { get; }

    private string _tag;
    private RabbitMqBroker _client;
    private EventingBasicConsumer _consumer;

    internal RabbitMqRpcServer(RabbitMqBroker client, IRpcServerOptions options)
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
            options.OnReceived?.Invoke(new OnReceivedEventArgs { Data = ea.Body.ToArray() });

            IRpcResponse response = null;
            var props = ea.BasicProperties;
            var replyProps = _client.Session.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

#if RELEASE
            try
            {
#endif
            if (options.OnRequest != null)
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var request = JsonConvert.DeserializeObject<BaseRpcRequest>(json);
                response = options.OnRequest(request);
            }
#if RELEASE
            } catch { }
#endif

            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            _client.Session.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: data);
        };
    }

    public void Connect() => _client.Connect();
    public void Disconnect() => _client.Disconnect();

    public void Start()
    {
        _client.Connect();
        _client.Session.BasicQos((uint)Options.QosPrefetchSize, (ushort)Options.QosPrefetchCount, Options.QosGlobal); // PrefetchCount is required
        _tag = _client.Session.BasicConsume(Options.QueueName, true, _consumer);
    }

    public void Stop()
    {
        _client.Session.BasicCancel(_tag);
    }
}
