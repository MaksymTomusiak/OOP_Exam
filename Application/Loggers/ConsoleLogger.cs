using Application.Wrapper;

namespace Application.Loggers;

public class ConsoleLogger(IConsoleWrapper consoleWrapper) : ILogger
{
    public void LogInfo(string message)
    {
        consoleWrapper.WriteLine($"[INFO] {message}");
    }

    public void LogError(Exception ex, string message)
    {
        consoleWrapper.WriteLine($"[ERROR] {message}\nDetails: {ex?.Message}");
    }
}