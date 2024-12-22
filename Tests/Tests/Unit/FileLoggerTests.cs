using Application.Loggers;
using Application.Wrapper;
using NSubstitute;
using System;
using System.IO;
using Xunit;

namespace Tests.Tests.Unit
{
    public class FileLoggerTests
    {
        [Fact]
        public void LogInfo_ShouldWriteInfoMessageToFile()
        {
            // Arrange
            var mockConsoleWrapper = Substitute.For<IConsoleWrapper>();
            var tempFilePath = Path.GetTempFileName();
            var logger = new FileLogger(tempFilePath, mockConsoleWrapper);
            var message = "Test info message";

            // Act
            logger.LogInfo(message);

            // Assert
            var logContent = File.ReadAllText(tempFilePath);
            Assert.Contains("[INFO]", logContent);
            Assert.Contains(message, logContent);

            // Cleanup
            File.Delete(tempFilePath);
        }

        [Fact]
        public void LogError_ShouldWriteErrorMessageAndExceptionToFile()
        {
            // Arrange
            var mockConsoleWrapper = Substitute.For<IConsoleWrapper>();
            var tempFilePath = Path.GetTempFileName();
            var logger = new FileLogger(tempFilePath, mockConsoleWrapper);
            var message = "Test error message";
            var exception = new Exception("Test exception");

            // Act
            logger.LogError(exception, message);

            // Assert
            var logContent = File.ReadAllText(tempFilePath);
            Assert.Contains("[ERROR]", logContent);
            Assert.Contains(message, logContent);
            Assert.Contains(exception.Message, logContent);

            // Cleanup
            File.Delete(tempFilePath);
        }

        [Fact]
        public void LogMessage_ShouldHandleDirectoryCreation()
        {
            // Arrange
            var mockConsoleWrapper = Substitute.For<IConsoleWrapper>();
            var tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var tempFilePath = Path.Combine(tempDirectory, "log.txt");
            var logger = new FileLogger(tempFilePath, mockConsoleWrapper);
            var message = "Test directory creation";

            // Act
            logger.LogInfo(message);

            // Assert
            Assert.True(Directory.Exists(tempDirectory));
            var logContent = File.ReadAllText(tempFilePath);
            Assert.Contains("[INFO]", logContent);
            Assert.Contains(message, logContent);

            // Cleanup
            Directory.Delete(tempDirectory, true);
        }

        [Fact]
        public void LogMessage_ShouldHandleLoggingFailure()
        {
            // Arrange
            var mockConsoleWrapper = Substitute.For<IConsoleWrapper>();
            var invalidFilePath = @"Z:\Invalid\Path\log.txt"; // Assuming this path is invalid
            var logger = new FileLogger(invalidFilePath, mockConsoleWrapper);
            var message = "Test logging failure";

            // Act
            logger.LogInfo(message);

            // Assert
            mockConsoleWrapper.Received(1).WriteLine(Arg.Is<string>(s => s.Contains("Failed to log message:")));
        }
    }
}
