namespace NanoBroker;

public interface IRpcClient
{
    public IRpcClientOptions Options { get; }

    public void Connect();
    public void Disconnect();

    public void Start();
    public void Stop();

    public IRpcResponse Call(IRpcRequest request);
    public Task<IRpcResponse> CallAsync(IRpcRequest request, CancellationToken ct = default);
}
