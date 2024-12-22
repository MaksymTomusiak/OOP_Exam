using Application;
using Infrastructure;
using Infrastructure.DbModule;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Api.HostCreation;

public static class HostCreator
{
    public static IHost Create(string[] args, Action<IHostBuilder>? configureHost = null)
    {
        string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
        string jsonFileName = Directory.GetFiles(directoryPath, "*.json").First();


        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile(jsonFileName, optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;

                services.AddInfrastructure(configuration);
                services.AddApplication();
            });
        configureHost?.Invoke(builder);
         
        var host = builder.Build();
        
        host.InitializeDb();

        return host;
    }
}