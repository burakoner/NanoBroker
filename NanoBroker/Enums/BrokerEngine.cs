namespace NanoBroker;

public enum BrokerEngine : byte
{
    /// <summary>
    /// Apache Active MQ
    /// </summary>
    ActiveMQ = 1,

    /// <summary>
    /// Amazon MQ
    /// </summary>
    AmazonMQ = 2,

    /// <summary>
    /// Amazon Simple Queue Service
    /// </summary>
    AmazonSQS = 3,

    /// <summary>
    /// Azure Service Bus
    /// </summary>
    AzureServiceBus = 4,

    /// <summary>
    /// Google Pub-Sub
    /// </summary>
    GooglePubSub = 5,

    /// <summary>
    /// Kafka
    /// </summary>
    Kafka = 6,

    /// <summary>
    /// NATS
    /// </summary>
    NATS = 7,

    /// <summary>
    /// RabbitMQ
    /// </summary>
    RabbitMQ = 8,

    /// <summary>
    /// Redis
    /// </summary>
    Redis = 9,
}
