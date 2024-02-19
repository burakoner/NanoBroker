using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NanoBroker.Engines.RabbitMQ;

public class RabbitMqRpcClient : IRpcClient
{
    public IRpcClientOptions Options { get; }

    private string _tag;
    private string _replyQueue;
    private RabbitMqBroker _client;
    private EventingBasicConsumer _consumer;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<IRpcResponse>> _callbacks = [];

    internal RabbitMqRpcClient(RabbitMqBroker client, IRpcClientOptions options)
    {
        _client = client;
        Options = options;
    }

    public void Connect() => _client.Connect();
    public void Disconnect() => _client.Disconnect();

    public void Start()
    {
        // Connect
        _client.Connect();

        // Declare a Server-Named Queue
        _replyQueue = _client.Session.QueueDeclare(queue: "").QueueName;

        // Consumer
        _consumer = new EventingBasicConsumer(_client.Session);
        _consumer.Received += (model, ea) =>
        {
#if RELEASE
            try
            {
#endif
                _client.Session.BasicAck(ea.DeliveryTag, true);

                if (!_callbacks.TryRemove(ea.BasicProperties.CorrelationId, out TaskCompletionSource<IRpcResponse> tcs))
                    return;

                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                var responseObject = JsonConvert.DeserializeObject<BaseRpcResponse>(response);
                tcs.TrySetResult(responseObject);
#if RELEASE
            }
            catch { }
#endif
        };

        // Start Consuming
        _tag = _client.Session.BasicConsume(_replyQueue, true, _consumer);
    }

    public void Stop()
    {
        _client.Session.BasicCancel(_tag);
    }

    public IRpcResponse Call(IRpcRequest request)
    {
        return CallAsync(request).GetAwaiter().GetResult();
    }

    public async Task<IRpcResponse> CallAsync(IRpcRequest request, CancellationToken ct = default)
    {
        // Task
        var task = CallRequestAsync(request, ct);

        // Has Timeout
        if (request.ResponseTimeout.HasValue && request.ResponseTimeout.Value.TotalMilliseconds > 0)
        {
            var timeout = Task.Delay(request.ResponseTimeout.Value, ct);
            var winner = await Task.WhenAny(task, timeout);
            if (winner == task)
            {
                // Success
                if (task.Result != null)
                    return task.Result;

                // Error
                return new BaseRpcResponse
                {
                    ResponseStatus = RpcResponseStatus.Failure,
                    ResponseError = new RpcResponseError
                    {
                        ErrorCode = "FAILURE",
                        ErrorMessage = "Result is NULL"
                    }
                };
            }
            else
            {
                return new BaseRpcResponse
                {
                    RequestId = request.RequestId,
                    ResponseStatus = RpcResponseStatus.Timeout,
                };
            }
        }

        // No Timeout
        return await task;
    }

    private Task<IRpcResponse> CallRequestAsync(IRpcRequest request, CancellationToken ct = default)
    {
        // Arrange
        var guid = Guid.NewGuid().ToString();
        var props = _client.Session.CreateBasicProperties();
        props.CorrelationId = guid;
        props.ReplyTo = _replyQueue;

        // Callback
        var tcs = new TaskCompletionSource<IRpcResponse>();
        _callbacks.TryAdd(guid, tcs);

        // Send Request
        var json = JsonConvert.SerializeObject(request);
        var bytes = Encoding.UTF8.GetBytes(json);
        _client.Session.BasicPublish(exchange: Options.ExchangeName, routingKey: Options.RouteName, basicProperties: props, body: bytes);

        // Cancellation Token
        ct.Register(() => _callbacks.TryRemove(guid, out var tmp));

        // Return
        return tcs.Task;
    }
}
