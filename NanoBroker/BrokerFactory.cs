using NanoBroker.Engines.RabbitMQ;

namespace NanoBroker;

public static class BrokerFactory
{
    public static IBrokerOptions BuildBrokerOptions(
        BrokerEngine engine, 
        string host, 
        int port, 
        string username, 
        string password, 
        string connectionString)
    {
        if (engine == BrokerEngine.RabbitMQ)
        {
            return new RabbitMqBrokerOptions
            {
                Host = host,
                Port = port,
                Username = username,
                Password = password,
                ConnectionString = connectionString,
            };
        }

        throw new NotImplementedException($"{nameof(engine)} is not implemented.");
    }
    public static IPublisherOptions BuildPublisherOptions(
        BrokerEngine engine, 
        string exchangeName,
        string exchangeType, 
        string queueName, 
        string queueType, 
        string routingKey,
        bool durable, 
        bool exclusive, 
        bool autoDelete, 
        IDictionary<string, object> arguments)
    {
        if (engine == BrokerEngine.RabbitMQ)
        {
            return new RabbitMqPublisherOptions
            {
                ExchangeName = exchangeName,
                ExchangeType = exchangeType,
                QueueName = queueName,
                QueueType = queueType,
                RouteName = routingKey,
                Durable = durable,
                Exclusive = exclusive,
                AutoDelete = autoDelete,
                Arguments = arguments,
            };
        }

        throw new NotImplementedException($"{nameof(engine)} is not implemented.");
    }
    public static IConsumerOptions BuildConsumerOptions(
        BrokerEngine engine,
        string exchangeName,
        string exchangeType,
        string queueName,
        string queueType,
        string routingKey,
        bool durable,
        bool exclusive,
        bool autoDelete,
        IDictionary<string, object> arguments,

        bool autoAcknowledgement,
        Action<OnRegisteredEventArgs> onRegistered,
        Action<OnUnregisteredEventArgs> onUnregistered,
        Action<OnConsumerCanceledEventArgs> onConsumerCanceled,
        Action<OnShutdownEventArgs> onShutdown,
        Action<OnReceivedEventArgs> onReceived)
    {
        if (engine == BrokerEngine.RabbitMQ)
        {
            return new RabbitMqConsumerOptions
            {
                ExchangeName = exchangeName,
                ExchangeType = exchangeType,
                QueueName = queueName,
                QueueType = queueType,
                RouteName = routingKey,
                Durable = durable,
                Exclusive = exclusive,
                AutoDelete = autoDelete,
                Arguments = arguments,

                AutoAcknowledgement = autoAcknowledgement,
                OnRegistered = onRegistered,
                OnUnregistered = onUnregistered,
                OnConsumerCanceled = onConsumerCanceled,
                OnShutdown = onShutdown,
                OnReceived = onReceived,
            };
        }

        throw new NotImplementedException($"{nameof(engine)} is not implemented.");
    }
    public static IStreamerOptions BuildStreamerOptions(
        BrokerEngine engine,
        string exchangeName,
        string exchangeType,
        string queueName,
        string queueType,
        string routingKey,
        bool durable,
        bool exclusive,
        bool autoDelete,
        IDictionary<string, object> arguments)
    {
        if (engine == BrokerEngine.RabbitMQ)
        {
            return new RabbitMqStreamerOptions
            {
                ExchangeName = exchangeName,
                ExchangeType = exchangeType,
                QueueName = queueName,
                QueueType = queueType,
                RouteName = routingKey,
                Durable = durable,
                Exclusive = exclusive,
                AutoDelete = autoDelete,
                Arguments = arguments,
            };
        }

        throw new NotImplementedException($"{nameof(engine)} is not implemented.");
    }
    public static IReceiverOptions BuildReceiverOptions(
        BrokerEngine engine,
        string exchangeName,
        string exchangeType,
        string queueName,
        string queueType,
        string routingKey,
        bool durable,
        bool exclusive,
        bool autoDelete,
        IDictionary<string, object> arguments,

        bool qosGlobal,
        int qosPrefetchSize,
        int qosPrefetchCount,

        Action<OnRegisteredEventArgs> onRegistered,
        Action<OnUnregisteredEventArgs> onUnregistered,
        Action<OnConsumerCanceledEventArgs> onConsumerCanceled,
        Action<OnShutdownEventArgs> onShutdown,
        Action<OnReceivedEventArgs> onReceived)
    {
        if (engine == BrokerEngine.RabbitMQ)
        {
            return new RabbitMqReceiverOptions
            {
                ExchangeName = exchangeName,
                ExchangeType = exchangeType,
                QueueName = queueName,
                QueueType = queueType,
                RouteName = routingKey,
                Durable = durable,
                Exclusive = exclusive,
                AutoDelete = autoDelete,
                Arguments = arguments,

                QosGlobal = qosGlobal,
                QosPrefetchSize = qosPrefetchSize,
                QosPrefetchCount = qosPrefetchCount,

                OnRegistered = onRegistered,
                OnUnregistered = onUnregistered,
                OnConsumerCanceled = onConsumerCanceled,
                OnShutdown = onShutdown,
                OnReceived = onReceived,
            };
        }

        throw new NotImplementedException($"{nameof(engine)} is not implemented.");
    }
    public static IRpcClientOptions BuildRpcClientOptions(
        BrokerEngine engine,
        string exchangeName,
        string exchangeType,
        string queueName,
        string queueType,
        string routingKey,
        bool durable,
        bool exclusive,
        bool autoDelete,
        IDictionary<string, object> arguments)
    {
        if (engine == BrokerEngine.RabbitMQ)
        {
            return new RabbitMqRpcClientOptions
            {
                ExchangeName = exchangeName,
                ExchangeType = exchangeType,
                QueueName = queueName,
                QueueType = queueType,
                RouteName = routingKey,
                Durable = durable,
                Exclusive = exclusive,
                AutoDelete = autoDelete,
                Arguments = arguments,
            };
        }

        throw new NotImplementedException($"{nameof(engine)} is not implemented.");
    }
    public static IRpcServerOptions BuildRpcServerOptions(
        BrokerEngine engine,
        string exchangeName,
        string exchangeType,
        string queueName,
        string queueType,
        string routingKey,
        bool durable,
        bool exclusive,
        bool autoDelete,
        IDictionary<string, object> arguments,

        bool qosGlobal,
        int qosPrefetchSize,
        int qosPrefetchCount,

        Action<OnRegisteredEventArgs> onRegistered,
        Action<OnUnregisteredEventArgs> onUnregistered,
        Action<OnConsumerCanceledEventArgs> onConsumerCanceled,
        Action<OnShutdownEventArgs> onShutdown,
        Action<OnReceivedEventArgs> onReceived,
        Func<IRpcRequest, IRpcResponse> onRequest)
    {
        if (engine == BrokerEngine.RabbitMQ)
        {
            return new RabbitMqRpcServerOptions
            {
                ExchangeName = exchangeName,
                ExchangeType = exchangeType,
                QueueName = queueName,
                QueueType = queueType,
                RouteName = routingKey,
                Durable = durable,
                Exclusive = exclusive,
                AutoDelete = autoDelete,
                Arguments = arguments,

                QosGlobal = qosGlobal,
                QosPrefetchSize = qosPrefetchSize,
                QosPrefetchCount = qosPrefetchCount,

                OnRegistered = onRegistered,
                OnUnregistered = onUnregistered,
                OnConsumerCanceled = onConsumerCanceled,
                OnShutdown = onShutdown,
                OnReceived = onReceived,
                OnRequest = onRequest,
            };
        }

        throw new NotImplementedException($"{nameof(engine)} is not implemented.");
    }

    public static IQueueBroker BuildQueueBroker(BrokerEngine engine, IBrokerOptions options)
    {
        return engine switch
        {
            BrokerEngine.RabbitMQ => new RabbitMqBroker(options),
            //MessageBroker.Kafka => new KafkaBroker(options),
            //MessageBroker.NATS => new NATSBroker(options),
            _ => throw new NotImplementedException($"{nameof(engine)} is not implemented.")
        };
    }
    public static IPublisher BuildPublisher(BrokerEngine engine, IBrokerOptions brokerOptions, IPublisherOptions publisherOptions)
    {
        if (engine == BrokerEngine.RabbitMQ)
        {
            var broker = new RabbitMqBroker(brokerOptions);
            return broker.BuildPublisher(publisherOptions);
        }

        throw new NotImplementedException($"{nameof(engine)} is not implemented.");
    }
    public static IConsumer BuildConsumer(BrokerEngine engine, IBrokerOptions brokerOptions, IConsumerOptions consumerOptions)
    {
        if (engine == BrokerEngine.RabbitMQ)
        {
            var broker = new RabbitMqBroker(brokerOptions);
            return broker.BuildConsumer(consumerOptions);
        }

        throw new NotImplementedException($"{nameof(engine)} is not implemented.");
    }

    public static IStreamBroker BuildStreamBroker(BrokerEngine engine, IBrokerOptions options)
    {
        return engine switch
        {
            BrokerEngine.RabbitMQ => new RabbitMqBroker(options),
            //MessageBroker.Kafka => new KafkaBroker(options),
            //MessageBroker.NATS => new NATSBroker(options),
            _ => throw new NotImplementedException($"{nameof(engine)} is not implemented.")
        };
    }
    public static IStreamer BuildStreamer(BrokerEngine engine, IBrokerOptions brokerOptions, IStreamerOptions streamerOptions)
    {
        if (engine == BrokerEngine.RabbitMQ)
        {
            var broker = new RabbitMqBroker(brokerOptions);
            return broker.BuildStreamer(streamerOptions);
        }

        throw new NotImplementedException($"{nameof(engine)} is not implemented.");
    }
    public static IReceiver BuildReceiver(BrokerEngine engine, IBrokerOptions brokerOptions, IReceiverOptions receiverOptions)
    {
        if (engine == BrokerEngine.RabbitMQ)
        {
            var broker = new RabbitMqBroker(brokerOptions);
            return broker.BuildReceiver(receiverOptions);
        }

        throw new NotImplementedException($"{nameof(engine)} is not implemented.");
    }

    public static IRpcBroker BuildRpcBroker(BrokerEngine engine, IBrokerOptions options)
    {
        return engine switch
        {
            BrokerEngine.RabbitMQ => new RabbitMqBroker(options),
            //MessageBroker.Kafka => new KafkaBroker(options),
            //MessageBroker.NATS => new NATSBroker(options),
            _ => throw new NotImplementedException($"{nameof(engine)} is not implemented.")
        };
    }
    public static IRpcClient BuildRpcClient(BrokerEngine engine, IBrokerOptions brokerOptions, IRpcClientOptions rpcClientOptions)
    {
        if (engine == BrokerEngine.RabbitMQ)
        {
            var broker = new RabbitMqBroker(brokerOptions);
            return broker.BuildRpcClient(rpcClientOptions);
        }

        throw new NotImplementedException($"{nameof(engine)} is not implemented.");
    }
    public static IRpcServer BuildRpcServer(BrokerEngine engine, IBrokerOptions brokerOptions, IRpcServerOptions rpcServerOptions)
    {
        if (engine == BrokerEngine.RabbitMQ)
        {
            var broker = new RabbitMqBroker(brokerOptions);
            return broker.BuildRpcServer(rpcServerOptions);
        }

        throw new NotImplementedException($"{nameof(engine)} is not implemented.");
    }
}
