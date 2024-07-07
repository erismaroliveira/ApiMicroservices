using ApiMicroservicesProduct.Dtos.Mappings;
using ApiMicroservicesProduct.Services;
using ApiMicroservicesProduct.Services.Interfaces;

namespace ApiMicroservicesProduct.Extensions;

public static class ServicesDependencyInjection
{
    public static IServiceCollection AddServicesDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<ICategoryDtoService, CategoryDtoService>();
        services.AddScoped<IProductDtoService, ProductDtoService>();

        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}
