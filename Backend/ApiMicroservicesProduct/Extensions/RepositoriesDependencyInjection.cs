using ApiMicroservicesProduct.Context.Repositories;
using ApiMicroservicesProduct.Models.Interfaces;

namespace ApiMicroservicesProduct.Extensions;

public static class RepositoriesDependencyInjection
{
    public static IServiceCollection AddRepositoriesDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
