using Microsoft.EntityFrameworkCore;

namespace MigrationApplier;

public class DbMigrator
{
    public void MigrateDatabase()
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        dbContext.Database.Migrate();

        Console.WriteLine("Database is migrated");
    }
}
