namespace NanoBroker;

public interface IRpcServer
{
    public IRpcServerOptions Options { get; }

    public void Connect();
    public void Disconnect();

    public void Start();
    public void Stop();
}
