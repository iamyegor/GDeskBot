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
            : Environment.GetEnvironmentVariable("CONNECTION_STRING")!;

        services.AddScoped(_ => new ApplicationDbContext(connectionString));
        services.AddLogging();

        return services;
    }
}
