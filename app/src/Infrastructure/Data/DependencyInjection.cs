using Infrastructure.Utils;
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
        string connectionString = EnvironmentResolver.IsDevelopment
            ? configuration.GetConnectionString(ConnectionString.Name)!
            : Environment.GetEnvironmentVariable(ConnectionString.Name)!;

        services.AddScoped(_ => new ApplicationDbContext(connectionString));

        return services;
    }
}
