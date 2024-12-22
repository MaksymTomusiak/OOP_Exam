using Application.Wrapper;

namespace Application.Loggers;
using System;
using System.IO;

public class FileLogger(string path, IConsoleWrapper consoleWrapper) : ILogger
{
    public void LogInfo(string message)
    {
        LogMessage("INFO", message);
    }

    public void LogError(Exception ex, string message)
    {
        var errorMessage = $"{message} - Exception: {ex?.Message}\n{ex?.StackTrace}";
        LogMessage("ERROR", errorMessage);
    }

    private void LogMessage(string logLevel, string message)
    {
        try
        {
            var directory = Path.GetDirectoryName(path);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var logMessage = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} [{logLevel}] {message}";
            File.AppendAllText(path, logMessage + Environment.NewLine);
        }
        catch (Exception ex)
        {
            consoleWrapper.WriteLine($"Failed to log message: {ex?.Message}");
        }
    }
}
