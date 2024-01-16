namespace NanoBroker;

public interface IPublisher
{
    public IPublisherOptions Options { get; }

    public void Connect();
    public void Disconnect();

    public void Publish(byte[] data);
    public void Publish(string data);
}
