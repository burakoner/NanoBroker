namespace NanoBroker;

public interface IRpcRequest
{
    public string RequestId { get; set; }
    public string RequestMethod { get; set; }
    public string RequestPayload { get; set; }
    public TimeSpan? ResponseTimeout { get; set; }
}
