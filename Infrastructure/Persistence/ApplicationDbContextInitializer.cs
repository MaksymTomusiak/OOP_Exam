using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContextInitializer(ApplicationDbContext context)
{
    public void Initialize()
    {
        context.Database.Migrate();
    }
}