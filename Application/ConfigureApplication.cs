using Application.Common.Interfaces;
using Application.Loggers;
using Application.Managers;
using Application.ManipulatorFactories;
using Application.Wrapper;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureApplication
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IConsoleWrapper, ConsoleWrapper>();
        services.AddSingleton<ILogger>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return LoggerFactory.LoggerFactory.CreateLogger(configuration, provider.GetRequiredService<IConsoleWrapper>());
        });
        services.AddSingleton<IManipulatorManager, ManipulatorManager>(provider =>
        {
            ManipulatorManager.Initialize(provider);
            return ManipulatorManager.Instance;
        });
        services.AddScoped<IndustrialManipulatorFactory>();
        services.AddScoped<ServiceManipulatorFactory>();
        services.AddScoped<Invoker.Invoker>();
    }
}