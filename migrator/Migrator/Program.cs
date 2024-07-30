using Microsoft.EntityFrameworkCore;
using Migrator;

ApplicationDbContext dbContext = new ApplicationDbContext();
dbContext.Database.Migrate();

Console.WriteLine("Database is migrated");