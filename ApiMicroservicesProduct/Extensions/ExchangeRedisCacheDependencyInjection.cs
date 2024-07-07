namespace ApiMicroservicesProduct.Extensions;

public static class ExchangeRedisCacheDependencyInjection
{
    public static IServiceCollection AddExchangeRedisCacheDependencyInjection(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "SampleInstance:";
        });

        services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });

        return services;
    }
}
