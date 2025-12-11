using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infra.Databases;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Para design-time (geração de migrations), usar SQL Server
        // A connection string será configurada no appsettings.json em runtime
        // Por enquanto, usando uma connection string padrão para design-time
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=ProductsCatalogDb;Trusted_Connection=true;MultipleActiveResultSets=true",
            b => b.MigrationsAssembly("Infra")
        );

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}

