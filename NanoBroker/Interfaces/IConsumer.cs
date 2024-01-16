namespace NanoBroker;

public interface IConsumer
{
    public IConsumerOptions Options { get; }

    public void Connect();
    public void Disconnect();

    public void Start();
    public void Stop();
}