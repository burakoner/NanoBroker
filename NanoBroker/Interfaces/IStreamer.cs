namespace NanoBroker;

public interface IStreamer
{
    public IStreamerOptions Options { get; }

    public void Connect();
    public void Disconnect();

    public void Stream(byte[] data);
    public void Stream(string data);
}
