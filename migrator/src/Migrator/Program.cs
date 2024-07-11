// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Migrator;



ApplicationContext context = new ApplicationContext();
context.Database.Migrate();

Console.WriteLine("Database is migrated");