using System;
using System.Collections.Generic;

namespace NanoBroker.Samples;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("1: Publisher");
        Console.WriteLine("2: Consumer");
        Console.WriteLine("");
        Console.WriteLine("3: Streamer");
        Console.WriteLine("4: Receiver");
        Console.WriteLine("");
        Console.WriteLine("5: RPC Client");
        Console.WriteLine("6: RPC Server");
        Console.WriteLine("");
        Console.Write("Choose RabbitMQ Role: ");

        var role = Console.ReadLine();
        if (role == "1") PublisherSample();
        if (role == "2") ConsumerSample();
        if (role == "3") StreamerSample();
        if (role == "4") ReceiverSample();
        if (role == "5") RpcClientSample();
        if (role == "6") RpcServerSample();
    }

    #region Publisher / Consumer
    static IBrokerOptions _pcOptions = BrokerFactory.BuildBrokerOptions(BrokerEngine.RabbitMQ,
        "localhost", 5672, "admin", "123456",
        "amqp://admin:123456@localhost:5672");

    static string _pcExchangeName = "pc-exchange";
    static string _pcExchangeType = "direct";
    static string _pcQueueName = "pc-queue";
    static string _pcQueueType = "";
    static string _pcRoutingKey = "pc-route";
    static bool _pcDurable = true;
    static bool _pcExclusive = false;
    static bool _pcAutoDelete = false;

    public static void PublisherSample()
    {
        Console.Clear();
        Console.Title = "RabbitMQ Publisher";
        Console.WriteLine("RabbitMQ Publisher.");
        Console.WriteLine("Press <ENTER> to start publishing!..");
        Console.ReadLine();

        var publisher = BrokerFactory.BuildPublisher(BrokerEngine.RabbitMQ, _pcOptions,
            BrokerFactory.BuildPublisherOptions(BrokerEngine.RabbitMQ,
            _pcExchangeName,
            _pcExchangeType,
            _pcQueueName,
            _pcQueueType,
            _pcRoutingKey,
            _pcDurable,
            _pcExclusive,
            _pcAutoDelete,
            null));

        for (var i = 1; i <= 10000; i++)
        {
            var text = $"Hello, World! {i}";
            Console.WriteLine($"Publishing: {text}");
            publisher.Publish(text);
        }

        Console.WriteLine("Done!");
        Console.ReadLine();
    }

    public static void ConsumerSample()
    {
        Console.Clear();
        Console.Title = "RabbitMQ Consumer";
        Console.WriteLine("RabbitMQ Consumer.");
        Console.WriteLine("Press <ENTER> to start consuming!..");
        Console.ReadLine();

        var consumer = BrokerFactory.BuildConsumer(BrokerEngine.RabbitMQ, _pcOptions,
            BrokerFactory.BuildConsumerOptions(BrokerEngine.RabbitMQ,
            _pcExchangeName,
            _pcExchangeType,
            _pcQueueName,
            _pcQueueType,
            _pcRoutingKey,
            _pcDurable,
            _pcExclusive,
            _pcAutoDelete,
            null,
            true,
            (args) =>
            {
                Console.WriteLine($"CONSUMER REGISTERED");
            },
            (args) =>
            {
                Console.WriteLine($"CONSUMER UNREGISTERED");
            },
            (args) =>
            {
                Console.WriteLine($"CONSUMER CANCELED");
            },
            (args) =>
            {
                Console.WriteLine($"CONSUMER SHUTDOWN");
            },
            (args) =>
            {
                var text = System.Text.Encoding.UTF8.GetString(args.Data);
                Console.WriteLine($"CONSUMER RECEIVED: {text}");
            }));

        consumer.Start();
        Console.WriteLine("Consumer started");
        Console.WriteLine("Press [ENTER] to exit.");
        Console.ReadLine();
    }
    #endregion

    #region Streamer / Receiver
    static IBrokerOptions _srOptions = BrokerFactory.BuildBrokerOptions(BrokerEngine.RabbitMQ,
        "localhost", 5672, "admin", "123456",
        "amqp://admin:123456@localhost:5672");

    static string _srExchangeName = "sr-exchange";
    static string _srExchangeType = "direct";
    static string _srQueueName = "sr-queue";
    static string _srQueueType = "";
    static string _srRoutingKey = "sr-route";
    static bool _srDurable = true;
    static bool _srExclusive = false;
    static bool _srAutoDelete = false;

    public static void StreamerSample()
    {
        Console.Clear();
        Console.Title = "RabbitMQ Streamer";
        Console.WriteLine("RabbitMQ Streamer.");
        Console.WriteLine("Press <ENTER> to start streaming!..");
        Console.ReadLine();

        var streamer = BrokerFactory.BuildStreamer(BrokerEngine.RabbitMQ, _srOptions, 
            BrokerFactory.BuildStreamerOptions(BrokerEngine.RabbitMQ,
            _srExchangeName,
            _srExchangeType,
            _srQueueName,
            _srQueueType,
            _srRoutingKey,
            _srDurable,
            _srExclusive,
            _srAutoDelete,

            new Dictionary<string, object>
            {
                ["x-max-age"] = "10s", // valid units: Y, M, D, h, m, s
                ["x-max-length-bytes"] = 20_000_000_000, // maximum stream size: 20 GB
                ["x-stream-max-segment-size-bytes"] = 100_000_000, // size of segment files: 100 MB
            }));

        for (var i = 1; i <= 10000; i++)
        {
            var text = $"Hello, World! {i}";
            Console.WriteLine($"Streaming: {text}");
            streamer.Stream(text);
        }

        Console.WriteLine("Done!");
        Console.ReadLine();
    }

    public static void ReceiverSample()
    {
        Console.Clear();
        Console.Title = "RabbitMQ Receiver";
        Console.WriteLine("RabbitMQ Receiver.");
        Console.WriteLine("Press <ENTER> to start receiving!..");
        Console.ReadLine();

        var receiver = BrokerFactory.BuildReceiver(BrokerEngine.RabbitMQ, _srOptions,
            BrokerFactory.BuildReceiverOptions(BrokerEngine.RabbitMQ,
            _srExchangeName,
            _srExchangeType,
            _srQueueName,
            _srQueueType,
            _srRoutingKey,
            _srDurable,
            _srExclusive,
            _srAutoDelete,

            new Dictionary<string, object>
            {
                ["x-max-age"] = "10s", // valid units: Y, M, D, h, m, s
                ["x-max-length-bytes"] = 20_000_000_000, // maximum stream size: 20 GB
                ["x-stream-max-segment-size-bytes"] = 100_000_000, // size of segment files: 100 MB
            },

            false,
            0,
            100,

            (args) =>
            {
                Console.WriteLine($"RECEIVER REGISTERED");
            },
            (args) =>
            {
                Console.WriteLine($"RECEIVER UNREGISTERED");
            },
            (args) =>
            {
                Console.WriteLine($"RECEIVER CANCELED");
            },
            (args) =>
            {
                Console.WriteLine($"RECEIVER SHUTDOWN");
            },
            (args) =>
            {
                var text = System.Text.Encoding.UTF8.GetString(args.Data);
                Console.WriteLine($"RECEIVER RECEIVED: {text}");
            }));

        receiver.Start();
        Console.WriteLine("Receiver started");
        Console.WriteLine("Press [ENTER] to exit.");
        Console.ReadLine();
    }
    #endregion

    #region RPC Server / Client
    static IBrokerOptions _rpcOptions = BrokerFactory.BuildBrokerOptions(BrokerEngine.RabbitMQ,
        "localhost", 5672, "admin", "123456",
        "amqp://admin:123456@localhost:5672");

    static string _rpcExchangeName = "rpc-exchange";
    static string _rpcExchangeType = "direct";
    static string _rpcQueueName = "rpc-queue";
    static string _rpcQueueType = "";
    static string _rpcRoutingKey = "rpc-route";
    static bool _rpcDurable = true;
    static bool _rpcExclusive = false;
    static bool _rpcAutoDelete = false;

    public static void RpcClientSample()
    {
        Console.Clear();
        Console.Title = "RabbitMQ RPC Client";
        Console.WriteLine("RabbitMQ RPC Client.");
        Console.WriteLine("Press <ENTER> to start sending!..");
        Console.ReadLine();

        var client = BrokerFactory.BuildRpcClient(BrokerEngine.RabbitMQ, _rpcOptions, new BaseRpcClientOptions
        {
            ExchangeName = _rpcExchangeName,
            ExchangeType = _rpcExchangeType,
            QueueName = _rpcQueueName,
            RouteName = _rpcRoutingKey,
            Durable = _rpcDurable,
            Exclusive = _rpcExclusive,
            AutoDelete = _rpcAutoDelete,
        });
        client.Start();

        for (int i = 1; i <= 1000; i++)
        {
            var message = $"Hello World! {i}";
            var response = client.Call(new BaseRpcRequest
            {
                RequestId = Guid.NewGuid().ToString(),
                RequestMethod = "ECHO",
                RequestPayload = message,
                // ResponseTimeout = TimeSpan.FromSeconds(5),
            });
            Console.WriteLine($"[x] Sent {message}");
            Console.WriteLine($"[.] Got  {response.ResponsePayload}");
            //Console.ReadLine();
        }

        Console.WriteLine("Done!");
        Console.ReadLine();
    }

    public static void RpcServerSample()
    {
        Console.Clear();
        Console.Title = "RabbitMQ RPC Server";
        Console.WriteLine("RabbitMQ RPC Server.");
        Console.WriteLine("Press <ENTER> to start listening!..");
        Console.ReadLine();

        var server = BrokerFactory.BuildRpcServer(BrokerEngine.RabbitMQ, _rpcOptions,
            BrokerFactory.BuildRpcServerOptions(BrokerEngine.RabbitMQ,
            _rpcExchangeName,
            _rpcExchangeType,
            _rpcQueueName,
            _rpcQueueType,
            _rpcRoutingKey,
            _rpcDurable,
            _rpcExclusive,
            _rpcAutoDelete,
            null,

            false,
            0,
            1,

            null,
            null,
            null,
            null,
            null,
            (request) =>
            {
                Console.WriteLine($"[.] Received {request.RequestPayload}");

                if (request.RequestMethod == "ECHO")
                {
                    return new BaseRpcResponse
                    {
                        RequestId = request.RequestId,
                        ResponsePayload = request.RequestPayload + " ANSWER",
                        ResponseStatus = RpcResponseStatus.Success,
                    };
                }

                return new BaseRpcResponse
                {
                    RequestId = request.RequestId,
                    ResponsePayload = "Invalid Method",
                    ResponseStatus = RpcResponseStatus.Failure,
                };
            }));

        server.Start();
        Console.WriteLine("RPC Server started");
        Console.WriteLine("Press [ENTER] to exit.");
        Console.ReadLine();
    }
    #endregion

}
