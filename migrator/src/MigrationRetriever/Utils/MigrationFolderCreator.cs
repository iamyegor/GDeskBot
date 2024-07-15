using MigrationRetriever.AppEnvironment;
using MigrationRetriever.Models;

namespace MigrationRetriever.Utils;

public class MigrationFolderCreator
{
    private readonly SolutionDirectoryResolver _solutionDirectoryResolver =
        new SolutionDirectoryResolver();

    public async Task CreateAsync(IReadOnlyList<MigrationFile> migrationFiles)
    {
        string migrationsFolderPath;
        if (EnvironmentResolver.IsDevelopment)
        {
            string solutionDir = _solutionDirectoryResolver.GetSolutionDirectoryPath();
            migrationsFolderPath = Path.Combine(solutionDir, "MigrationApplier", "Migrations");
        }
        else
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            migrationsFolderPath = Path.Combine(baseDirectory, "MigrationApplier", "Migrations");
        }

        if (Directory.Exists(migrationsFolderPath))
        {
            Directory.Delete(migrationsFolderPath, true);
        }

        Directory.CreateDirectory(migrationsFolderPath);

        foreach (var migrationFile in migrationFiles)
        {
            string filePath = Path.Combine(migrationsFolderPath, migrationFile.FileName);
            await File.WriteAllTextAsync(filePath, migrationFile.Content);
        }
    }
}
