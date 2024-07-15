// See https://aka.ms/new-console-template for more information

using Common.AppEnvironment;
using Migrator.Models;
using Migrator.Utils;

MigrationFilesProcessor filesProcessor = new MigrationFilesProcessor();
MigrationFilesResolver filesResolver = new MigrationFilesResolver();
MigrationFolderCreator folderCreator = new MigrationFolderCreator();
ApplicationRunner applicationRunner = new ApplicationRunner();

string appAddress = EnvironmentResolver.IsProduction
    ? Environment.GetEnvironmentVariable("APP_ADDRESS")!
    : "localhost:1402";

string endpointUrl = $"http://{appAddress}/migrations";

List<MigrationFile> migrationFiles = await filesResolver.GetMigrationsAsync(endpointUrl);

IReadOnlyList<MigrationFile> proccessedMigrationFiles = filesProcessor.Proccess(migrationFiles);

await folderCreator.CreateAsync(proccessedMigrationFiles);

applicationRunner.RunMigrationApplier();