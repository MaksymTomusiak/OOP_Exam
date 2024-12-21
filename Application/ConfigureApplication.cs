using Application.Loggers;
using Application.Managers;
using Application.Wrapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureApplication
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IConsoleWrapper, ConsoleWrapper>();
        services.AddSingleton<ILogger>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return LoggerFactory.LoggerFactory.CreateLogger(configuration, provider.GetRequiredService<IConsoleWrapper>());
        });
        services.AddSingleton<IManipulatorManager, ManipulatorManager>();
    }
}