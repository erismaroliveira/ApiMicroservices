using ApiMicroservicesProduct.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiMicroservicesProduct.Extensions;

public static class DbContextDependencyInjection
{
    public static IServiceCollection AddDbContextDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            x => x.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
    }
}
