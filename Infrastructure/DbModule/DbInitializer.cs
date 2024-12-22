using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.DbModule;

public static class DbInitializer
{
    public static void InitializeDb(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var provider = scope.ServiceProvider;
        var initializer = provider.GetRequiredService<ApplicationDbContextInitializer>();

        initializer.Initialize();
    }
}