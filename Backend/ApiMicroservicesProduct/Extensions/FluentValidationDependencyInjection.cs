using ApiMicroservicesProduct.Dtos;
using ApiMicroservicesProduct.FluentValidation.CategoryDtoValidationLibrary;
using ApiMicroservicesProduct.FluentValidation.ProductDtoValidationLibrary;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace ApiMicroservicesProduct.Extensions;

public static class FluentValidationDependencyInjection
{
    public static IServiceCollection AddFluentValidationDependencyInjection(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CategoryDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<ProductDtoValidator>();
        services.AddFluentValidationAutoValidation();

        services.AddScoped<IValidator<CategoryDto>, CategoryDtoValidator>();
        services.AddScoped<IValidator<ProductDto>, ProductDtoValidator>();

        return services;
    }
}
