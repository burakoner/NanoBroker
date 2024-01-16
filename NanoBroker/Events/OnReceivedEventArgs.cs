namespace NanoBroker;

public class OnReceivedEventArgs : EventArgs
{
    public byte[] Data { get; set; }

    public string GetDataText(Encoding encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return encoding.GetString(Data);
    }

    public T GetDataObject<T>(Encoding encoding = null)
    {
        encoding ??= Encoding.UTF8;
        var json = encoding.GetString(Data);
        return JsonConvert.DeserializeObject<T>(json);
    }
}
