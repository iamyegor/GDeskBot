using System.Diagnostics;
using MigrationRetriever.AppEnvironment;
using Serilog;

namespace MigrationRetriever.Utils;

public class ApplicationRunner
{
    private readonly SolutionDirectoryResolver _solutionDirectoryResolver =
        new SolutionDirectoryResolver();

    public void RunMigrationApplier()
    {
        string appProjectPath = "MigrationApplier";
        string appExecutablePath = "MigrationApplier/bin/Debug/net8.0/MigrationApplier";
        if (EnvironmentResolver.IsDevelopment)
        {
            string solutionDirectory = _solutionDirectoryResolver.GetSolutionDirectoryPath();
            appProjectPath = Path.Combine(solutionDirectory, appProjectPath);
            appExecutablePath = Path.Combine(solutionDirectory, appExecutablePath);
        }

        BuildApp(appProjectPath);
        RunApp(appExecutablePath);
    }

    private void BuildApp(string projectPath)
    {
        ProcessStartInfo buildInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"build {projectPath}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process buildProcess = Process.Start(buildInfo)!)
        {
            string buildOutput = buildProcess.StandardOutput.ReadToEnd();
            string buildError = buildProcess.StandardError.ReadToEnd();

            buildProcess.WaitForExit();

            Log.Information("BuildProccess: {@build}", buildProcess);
            if (buildProcess.ExitCode != 0)
            {
                Console.WriteLine($"ExitCode: {buildProcess.ExitCode}");
                Console.WriteLine($"Build output: {buildOutput}");
                Console.WriteLine($"BuildError: {buildError}");
                throw new Exception(buildError);
            }
            else
            {
                Console.WriteLine("Build succeeded:");
                Console.WriteLine(buildOutput);
            }
        }
    }

    private void RunApp(string executablePath)
    {
        ProcessStartInfo runInfo = new ProcessStartInfo
        {
            FileName = executablePath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process runProcess = Process.Start(runInfo)!)
        {
            string runOutput = runProcess.StandardOutput.ReadToEnd();
            string runError = runProcess.StandardError.ReadToEnd();

            runProcess.WaitForExit();

            Console.WriteLine("Output from MigrationApplier:");
            Console.WriteLine(runOutput);

            if (!string.IsNullOrEmpty(runError))
            {
                Console.WriteLine($"RunError: {runError}");
                throw new Exception(runError);
            }
        }
    }
}
