namespace NanoBroker;

public class BaseBrokerOptions : IBrokerOptions
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ConnectionString { get; set; }
}
