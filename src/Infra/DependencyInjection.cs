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
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ProductsCatalogDb"));
        }
        else if (connectionString.Contains("Host=") || connectionString.Contains("Server=") && connectionString.Contains("Port="))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Infra")));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Infra")));
        }

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        return services;
    }
}