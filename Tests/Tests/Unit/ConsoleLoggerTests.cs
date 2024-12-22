using Application.Loggers;
using Application.Wrapper;
using NSubstitute;
using System;
using Xunit;

namespace Tests.Tests.Unit
{
    public class ConsoleLoggerTests
    {
        [Fact]
        public void LogInfo_ShouldWriteInfoMessageToConsole()
        {
            // Arrange
            var consoleWrapper = Substitute.For<IConsoleWrapper>();
            var logger = new ConsoleLogger(consoleWrapper);
            var message = "Test info message";

            // Act
            logger.LogInfo(message);

            // Assert
            consoleWrapper.Received(1).WriteLine($"[INFO] {message}");
        }

        [Fact]
        public void LogError_ShouldWriteErrorMessageAndExceptionToConsole()
        {
            // Arrange
            var consoleWrapper = Substitute.For<IConsoleWrapper>();
            var logger = new ConsoleLogger(consoleWrapper);
            var message = "Test error message";
            var exception = new Exception("Test exception");

            // Act
            logger.LogError(exception, message);

            // Assert
            consoleWrapper.Received(1).WriteLine($"[ERROR] {message}\nDetails: {exception.Message}");
        }
    }
}
