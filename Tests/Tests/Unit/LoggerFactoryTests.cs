using Application.LoggerFactory;
using Application.Loggers;
using Application.Wrapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace Tests.Tests.Unit
{
    public class LoggerFactoryTests
    {
        [Fact]
        public void CreateLogger_ShouldReturnConsoleLogger_WhenConfiguredForConsole()
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();
            configuration["Logger:LoggerType"].Returns("Console");
            var consoleWrapper = Substitute.For<IConsoleWrapper>();

            // Act
            var logger = LoggerFactory.CreateLogger(configuration, consoleWrapper);

            // Assert
            logger.Should().NotBeNull();
            logger.Should().BeOfType<ConsoleLogger>();
        }

        [Fact]
        public void CreateLogger_ShouldReturnFileLogger_WhenConfiguredForFile()
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();
            var tempFilePath = Path.GetTempFileName();
            configuration["Logger:LoggerType"].Returns("File");
            configuration["Logger:FilePath"].Returns(tempFilePath);
            var consoleWrapper = Substitute.For<IConsoleWrapper>();

            // Act
            var logger = LoggerFactory.CreateLogger(configuration, consoleWrapper);

            // Assert
            logger.Should().NotBeNull();
            logger.Should().BeOfType<FileLogger>();
        }

        [Fact]
        public void CreateLogger_ShouldThrowInvalidOperationException_WhenLoggerTypeIsInvalid()
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();
            configuration["Logger:LoggerType"].Returns("InvalidLoggerType");
            var consoleWrapper = Substitute.For<IConsoleWrapper>();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                LoggerFactory.CreateLogger(configuration, consoleWrapper)
            );
            Assert.Equal("Invalid logger type in configuration.", exception.Message);
        }

        [Fact]
        public void FileLogger_ShouldLogMessageToFile_WhenConfiguredForFile()
        {
            // Arrange
            var tempFilePath = Path.GetTempFileName();
            var configuration = Substitute.For<IConfiguration>();
            configuration["Logger:LoggerType"].Returns("File");
            configuration["Logger:FilePath"].Returns(tempFilePath);
            var consoleWrapper = Substitute.For<IConsoleWrapper>();
            var logger = LoggerFactory.CreateLogger(configuration, consoleWrapper);
            var message = "Test message for FileLogger";

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
        public void LoggerFactory_ShouldHandleInvalidLoggerTypeInConfiguration()
        {
            // Arrange
            var configuration = Substitute.For<IConfiguration>();
            configuration["Logger:LoggerType"].Returns("InvalidLoggerType");
            var consoleWrapper = Substitute.For<IConsoleWrapper>();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                LoggerFactory.CreateLogger(configuration, consoleWrapper)
            );
            Assert.Equal("Invalid logger type in configuration.", exception.Message);
        }
    }
}
