using Application.Loggers;
using Application.Wrapper;
using Microsoft.Extensions.Configuration;

namespace Application.LoggerFactory
{
    public static class LoggerFactory
    {
        private const string ConsoleLoggerName = "Console";
        private const string FileLoggerName = "File";
        
        private const string LoggerTypePath = "Logger:LoggerType";
        private const string LoggerFilepath = "Logger:FilePath";

        private const string InvalidLoggerMessage = "Invalid logger type in configuration.";
        public static ILogger CreateLogger(IConfiguration configuration, IConsoleWrapper consoleWrapper)
        {
            var loggerType = configuration[LoggerTypePath];
            
            return loggerType switch
            {
                ConsoleLoggerName => new ConsoleLogger(consoleWrapper),
                FileLoggerName => new FileLogger(configuration[LoggerFilepath], consoleWrapper),
                _ => throw new InvalidOperationException(InvalidLoggerMessage)
            };
        }
    }
}
