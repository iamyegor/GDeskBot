using Common.AppEnvironment;
using Microsoft.EntityFrameworkCore;

namespace MigrationApplier;

public class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = EnvironmentResolver.IsProduction
            ? Environment.GetEnvironmentVariable("CONNECTION_STRING")!
            : "Host=localhost;Port=5432;Database=bot_db;Username=postgres;Password=yegor";
        
        optionsBuilder.UseNpgsql(connectionString);
    }
}
