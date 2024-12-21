using Application;
using Application.Common.Interfaces;
using Application.Managers;
using Domain;
using Domain.Enums;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddInfrastructure(configuration);
        services.AddApplication();
    })
    .Build();

var scope = host.Services.CreateScope();

var serviceProvider = scope.ServiceProvider;

var manager = serviceProvider.GetService<IManipulatorManager>();

Console.WriteLine(manager?.GetAllManipulators<ServiceManipulator>().First().Name);

Console.ReadLine();