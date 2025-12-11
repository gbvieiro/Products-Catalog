using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infra.Databases;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=product_catalog;Username=postgres;Password=admin",
            b => b.MigrationsAssembly("Infra")
        );

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}

