using System.Reflection;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<BaseManipulator> BaseManipulators { get; set; }
    public DbSet<IndustrialManipulator> IndustrialManipulators { get; set; }
    public DbSet<ServiceManipulator> ServiceManipulators { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<BaseManipulator>()
            .UseTptMappingStrategy();
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}