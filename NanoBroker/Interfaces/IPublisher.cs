namespace NanoBroker;

public interface IPublisher
{
    public IPublisherOptions Options { get; }

    public void Connect();
    public void Disconnect();

    public void Publish(byte[] data);
    public void Publish(string data);

    public Task PublishAsync(byte[] data, CancellationToken ct = default);
    public Task PublishAsync(string data, CancellationToken ct = default);
}
