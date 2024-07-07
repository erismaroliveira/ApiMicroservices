namespace ApiMicroservicesProduct.Extensions;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructureModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddDbContextDependencyInjection(configuration)
            .AddRepositoriesDependencyInjection()
            .AddServicesDependencyInjection()
            .AddExchangeRedisCacheDependencyInjection();
    }
}
