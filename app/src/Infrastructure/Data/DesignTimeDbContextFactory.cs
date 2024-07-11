using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

    public ApplicationDbContext CreateDbContext(string[] args)
    {
        char separator = Path.DirectorySeparatorChar;
        string basePath = Directory.GetCurrentDirectory() + $"{separator}..{separator}Api";

        Console.WriteLine($"basePath: {basePath}");

        return Create(basePath, Environment.GetEnvironmentVariable(AspNetCoreEnvironment));
    }

    private ApplicationDbContext Create(string basePath, string? environmentName)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            // .AddJsonFile("appsettings.json")
            // .AddJsonFile("appsettings.Local.json", optional: true)
            // .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .AddJsonFile($"appsettings.local.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        string? connectionString = configuration.GetConnectionString(ConnectionString.Name);

        return Create(connectionString);
    }

    private ApplicationDbContext Create(string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException(
                $"Connection string '{ConnectionString.Name}' is null or empty.",
                nameof(connectionString)
            );
        }

        Console.WriteLine(
            $"DesignTimeDbContextFactoryBase.Create(string): Connection string: '{connectionString}'."
        );

        return new ApplicationDbContext(connectionString);
    }
}
