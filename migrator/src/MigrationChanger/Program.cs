// See https://aka.ms/new-console-template for more information

Console.WriteLine("Enter the directory path:");
string? directoryPath = Console.ReadLine();

if (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
{
    Console.WriteLine("Invalid directory path.");
    return;
}

ProcessDirectory(directoryPath);
Console.WriteLine("Processing complete.");

static void ProcessDirectory(string directoryPath)
{
    string[] files = Directory.GetFiles(
        directoryPath,
        "*.Designer.cs",
        SearchOption.AllDirectories
    );

    foreach (string filePath in files)
    {
        ProcessFile(filePath);
    }
}

static void ProcessFile(string filePath)
{
    string[] lines = File.ReadAllLines(filePath);
    bool updated = false;

    for (int i = 0; i < Math.Min(10, lines.Length); i++)
    {
        if (lines[i].Contains("using Infrastructure.Data;"))
        {
            lines[i] = lines[i].Replace("using Infrastructure.Data;", "using Migrator;");
            updated = true;
            break;
        }
    }

    if (updated)
    {
        File.WriteAllLines(filePath, lines);
        Console.WriteLine($"Updated file: {filePath}");
    }
}
