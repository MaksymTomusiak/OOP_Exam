using Api.HostCreation;
using Application.Invoker;
using Microsoft.Extensions.DependencyInjection;

var host = HostCreator.Create(args);

var scope = host.Services.CreateScope();

var serviceProvider = scope.ServiceProvider;

var invoker = serviceProvider.GetRequiredService<Invoker>();

invoker.Run();