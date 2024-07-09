using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped(_ => new ApplicationDbContext(
            configuration.GetConnectionString(ConnectionString.Name)!
        ));
        
        return services;
    }
}
