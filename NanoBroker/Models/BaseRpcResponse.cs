namespace NanoBroker;

public class BaseRpcResponse : IRpcResponse
{
    public string RequestId { get; set; }
    public string ResponsePayload { get; set; }
    public IRpcResponseError ResponseError { get; set; }
    public RpcResponseStatus ResponseStatus { get; set; }
    public Dictionary<string, object> ResponseArguments { get; set; } = [];
}

public class RpcResponseError: IRpcResponseError
{
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }

    public RpcResponseError() { }
    public RpcResponseError(int errorCode) : this(errorCode.ToString()) { }
    public RpcResponseError(int errorCode, string errorMessage) : this(errorCode.ToString(), errorMessage) { }
    public RpcResponseError(string errorCode) : this(errorCode, null) { }
    public RpcResponseError(string errorCode, string errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}