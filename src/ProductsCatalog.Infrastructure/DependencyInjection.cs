using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsCatalog.Application.Common.Interfaces;
using ProductsCatalog.Domain.Repositories;
using ProductsCatalog.Infrastructure.Persistence;
using ProductsCatalog.Infrastructure.Persistence.Repositories;
using ProductsCatalog.Infrastructure.Services;

namespace ProductsCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ProductsCatalogDb"));
        }
        else if (connectionString.Contains("Host=") || (connectionString.Contains("Server=") && connectionString.Contains("Port=")))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly("ProductsCatalog.Infrastructure")));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("ProductsCatalog.Infrastructure")));
        }

        // Mesma instancia de DbContext do escopo atual, exposta como a porta IUnitOfWork.
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddSingleton<IDateTime, DateTimeService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
