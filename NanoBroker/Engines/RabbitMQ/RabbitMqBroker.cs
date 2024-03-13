using RabbitMQ.Client;

namespace NanoBroker.Engines.RabbitMQ;

public class RabbitMqBroker(IBrokerOptions options) : IQueueBroker, IStreamBroker, IRpcBroker
{
    public IBrokerOptions Options { get; set; } = options;

    #region Connection
    private IConnection _connection;
    public IConnection Connection
    {
        get
        {
            if (_connection is null || !_connection.IsOpen) Connect();
            return _connection;
        }
    }
    public void Connect()
    {
        if (_connection is null || !_connection.IsOpen)
        {
            // Arrange
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(Options.ConnectionString),
                HostName = Options.Host,
                Port = Options.Port,
                UserName = Options.Username,
                Password = Options.Password,
                RequestedHeartbeat = TimeSpan.FromSeconds(30.0),
            };

            // Action
            _connection = connectionFactory.CreateConnection();
            _session = _connection.CreateModel();
        }
    }
    public void Disconnect()
    {
        Session.Close();
        Connection.Close();
    }
    #endregion

    #region Session
    private IModel _session;
    public IModel Session
    {
        get
        {
            return _session;
        }
    }
    #endregion

    #region Queue
    public IPublisher BuildPublisher(IPublisherOptions options)
    {
        // Connect
        Connect();

        // Exchange
        Session.ExchangeDeclare(options.ExchangeName, options.ExchangeType, options.Durable, options.AutoDelete, options.Arguments);

        // Queue
        Session.QueueDeclare(options.QueueName, options.Durable, options.Exclusive, options.AutoDelete, options.Arguments);

        // Queue Binding
        Session.QueueBind(options.QueueName, options.ExchangeName, options.RouteName, options.Arguments);

        // Publisher
        var publisher = new RabbitMqPublisher(this, options);

        // Return
        return publisher;
    }

    public IConsumer BuildConsumer(IConsumerOptions options)
    {
        // Connect
        Connect();

        // Exchange
        Session.ExchangeDeclare(options.ExchangeName, options.ExchangeType, options.Durable, options.AutoDelete, options.Arguments);

        // Queue
        Session.QueueDeclare(options.QueueName, options.Durable, options.Exclusive, options.AutoDelete, options.Arguments);

        // Queue Binding
        Session.QueueBind(options.QueueName, options.ExchangeName, options.RouteName, options.Arguments);

        // Consumer
        var consumer = new RabbitMqConsumer(this, options);

        // Return
        return consumer;
    }
    #endregion

    #region Stream
    public IStreamer BuildStreamer(IStreamerOptions options)
    {
        // Connect
        Connect();

        // Exchange
        Session.ExchangeDeclare(options.ExchangeName, options.ExchangeType, options.Durable, options.AutoDelete, options.Arguments);

        // Arguments
        if (options.Arguments is null) options.Arguments = [];
        options.Arguments["x-queue-type"] = "stream";

        // Queue
        Session.QueueDeclare(options.QueueName, options.Durable, options.Exclusive, options.AutoDelete, options.Arguments);

        // Queue Binding
        Session.QueueBind(options.QueueName, options.ExchangeName, options.RouteName, options.Arguments);

        // Streamer
        var streamer = new RabbitMqStreamer(this, options);

        // Return
        return streamer;
    }

    public IReceiver BuildReceiver(IReceiverOptions options)
    {
        // Connect
        Connect();

        // Exchange
        Session.ExchangeDeclare(options.ExchangeName, options.ExchangeType, options.Durable, options.AutoDelete, options.Arguments);

        // Arguments
        options.Arguments ??= [];
        options.Arguments["x-queue-type"] = "stream";

        // Queue
        Session.QueueDeclare(options.QueueName, options.Durable, options.Exclusive, options.AutoDelete, options.Arguments);

        // Queue Binding
        Session.QueueBind(options.QueueName, options.ExchangeName, options.RouteName, options.Arguments);

        // Receiver
        var receiver = new RabbitMqReceiver(this, options);

        // Return
        return receiver;
    }
    #endregion

    #region RPC
    public IRpcClient BuildRpcClient(IRpcClientOptions options)
    {
        // Connect
        Connect();

        // Exchange
        Session.ExchangeDeclare(options.ExchangeName, options.ExchangeType, options.Durable, options.AutoDelete, options.Arguments);

        // Queue
        Session.QueueDeclare(options.QueueName, options.Durable, options.Exclusive, options.AutoDelete, options.Arguments);

        // Queue Binding
        Session.QueueBind(options.QueueName, options.ExchangeName, options.RouteName, options.Arguments);

        // Client
        var client = new RabbitMqRpcClient(this, options);

        // Return
        return client;
    }

    public IRpcServer BuildRpcServer(IRpcServerOptions options)
    {
        // Connect
        Connect();

        // Exchange
        Session.ExchangeDeclare(options.ExchangeName, options.ExchangeType, options.Durable, options.AutoDelete, options.Arguments);

        // Queue
        Session.QueueDeclare(options.QueueName, options.Durable, options.Exclusive, options.AutoDelete, options.Arguments);

        // Queue Binding
        Session.QueueBind(options.QueueName, options.ExchangeName, options.RouteName, options.Arguments);

        // Server
        var server = new RabbitMqRpcServer(this, options);

        // Return
        return server;
    }
    #endregion
}
