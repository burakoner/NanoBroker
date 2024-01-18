namespace NanoBroker;

public interface IStreamer
{
    public IStreamerOptions Options { get; }

    public void Connect();
    public void Disconnect();

    public void Stream(byte[] data);
    public void Stream(string data);

    public Task StreamAsync(byte[] data, CancellationToken ct = default);
    public Task StreamAsync(string data, CancellationToken ct = default);
}
