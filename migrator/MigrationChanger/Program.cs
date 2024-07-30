using System.Text;

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
    string[] files = Directory.GetFiles(directoryPath);

    foreach (string filePath in files)
    {
        ProcessFile(filePath);
    }
}

static void ProcessFile(string filePath)
{
    StringBuilder fileText = new (File.ReadAllText(filePath));

    fileText.Replace("using Infrastructure.Data;", "using Migrator;");
    fileText.Replace("ApplicationContext", "ApplicationDbContext");

    File.WriteAllText(filePath, fileText.ToString());
    Console.WriteLine($"Updated file: {filePath}");
}
