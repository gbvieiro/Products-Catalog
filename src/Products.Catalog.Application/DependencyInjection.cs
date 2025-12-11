using Microsoft.Extensions.DependencyInjection;
using Products.Catalog.Application.Services.Books;
using Products.Catalog.Application.Services.Orders;
using Products.Catalog.Application.Services.Stocks;
using Products.Catalog.Application.Services.Users;

namespace Product.Catalog.Infra.IOC;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddScoped<IBooksAppService, BooksAppService>();
        services.AddScoped<IOrdersAppService, OrdersAppService>();
        services.AddScoped<IStocksAppService, StocksAppService>();
        services.AddScoped<IUsersAppService, UsersAppService>();
        
        return services;
    }
}