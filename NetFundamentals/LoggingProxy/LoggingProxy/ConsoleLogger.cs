using LoggingProxy.Interfaces;

namespace LoggingProxy;

public class ConsoleLogger : ILogger 
{
    public void Log(string message)
    {
        Console.WriteLine($"{DateTime.Now} {message}");
    }
}
