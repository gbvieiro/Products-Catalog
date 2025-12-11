using Infra.Databases;
using Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Catalog.Domain.Interfaces;

namespace Product.Catalog.Infra.IOC;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfra(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurar ApplicationDbContext
        // Lê a connection string do appsettings.json
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            // Se não houver connection string configurada, usa InMemory Database (desenvolvimento)
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ProductsCatalogDb"));
        }
        else
        {
            // Usa SQL Server com a connection string configurada
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Infra")));
        }

        // Registrar repositório genérico
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        return services;
    }
}