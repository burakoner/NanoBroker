namespace NanoBroker;

public interface IBroker
{
    public IBrokerOptions Options { get; }
}

public interface IQueueBroker : IBroker
{
    public IPublisher BuildPublisher(IPublisherOptions options);
    public IConsumer BuildConsumer(IConsumerOptions options);
}

public interface IStreamBroker : IBroker
{
    public IStreamer BuildStreamer(IStreamerOptions options);
    public IReceiver BuildReceiver(IReceiverOptions options);
}

public interface IRpcBroker : IBroker
{
    public IRpcServer BuildRpcServer(IRpcServerOptions options);
    public IRpcClient BuildRpcClient(IRpcClientOptions options);
}
