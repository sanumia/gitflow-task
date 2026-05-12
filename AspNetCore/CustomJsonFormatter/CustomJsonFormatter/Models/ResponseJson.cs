namespace CustomJsonFormatter.Models;

public class ResponseJson<T>
{
    public T Data { get; set; }
    public Dictionary<string, string> Links { get; set; }
}
