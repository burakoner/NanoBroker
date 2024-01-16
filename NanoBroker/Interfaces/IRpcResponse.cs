namespace NanoBroker;

public interface IRpcResponse
{
    public string RequestId { get; set; }
    public string ResponsePayload { get; set; }
    public IRpcResponseError ResponseError { get; set; }
    public RpcResponseStatus ResponseStatus { get; set; }
    public Dictionary<string, object> ResponseArguments { get; set; }
}

public interface IRpcResponseError
{
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}