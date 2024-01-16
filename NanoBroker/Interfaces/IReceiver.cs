namespace NanoBroker;

public interface IReceiver
{
    public IReceiverOptions Options { get; }

    public void Connect();
    public void Disconnect();

    public void Start();
    public void Stop();
}
