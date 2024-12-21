namespace Application.Loggers;

public interface ILogger
{
    void LogInfo(string message);
    void LogError(Exception ex, string message);
}