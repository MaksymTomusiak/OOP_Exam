using Application.Common.Interfaces;
using Domain;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Persistence;

public static class ConfigurePersistence
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var dataSourceBuild = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Default"));
        dataSourceBuild.EnableDynamicJson();
        var dataSource = dataSourceBuild.Build();

        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseNpgsql(
                    dataSource,
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));

        services.AddScoped<ApplicationDbContextInitializer>();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<BaseManipulatorRepository>();
        services.AddScoped<IRepository<BaseManipulator>>(
            provider => provider.GetRequiredService<BaseManipulatorRepository>());
        
        services.AddScoped<ServiceManipulatorRepository>();
        services.AddScoped<IRepository<ServiceManipulator>>(
            provider => provider.GetRequiredService<ServiceManipulatorRepository>());
        
        services.AddScoped<IndustrialManipulatorRepository>();
        services.AddScoped<IRepository<IndustrialManipulator>>(
            provider => provider.GetRequiredService<IndustrialManipulatorRepository>());
    }
}