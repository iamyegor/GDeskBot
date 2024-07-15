using MigrationRetriever.Models;

namespace MigrationRetriever.Utils;

public class MigrationFilesProcessor
{
    public IReadOnlyList<MigrationFile> Proccess(List<MigrationFile> migrationFiles)
    {
        List<MigrationFile> migrationFilesToReturn = [];
        foreach (MigrationFile migrationFile in migrationFiles)
        {
            migrationFilesToReturn.Add(Proccess(migrationFile));
        }

        return migrationFilesToReturn;
    }

    private MigrationFile Proccess(MigrationFile migrationFile)
    {
        string input = migrationFile.Content;
        string[] lines = input.Split(["\n"], StringSplitOptions.None);
        bool updated = false;

        for (int i = 0; i < Math.Min(20, lines.Length); i++)
        {
            if (lines[i].Contains("using Infrastructure.Data;"))
            {
                lines[i] = lines[i]
                    .Replace("using Infrastructure.Data;", "using MigrationApplier;");
                
                lines[i] = lines[i]
                    .Replace("typeof(ApplicationContext)", "typeof(ApplicationDbContext)");

                updated = true;
                break;
            }
        }

        if (updated)
        {
            migrationFile.Content = string.Join("\n", lines);
        }

        return migrationFile;
    }
}
