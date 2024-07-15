namespace MigrationRetriever.AppEnvironment;

public class EnvironmentResolver
{
    public static bool IsDevelopment => !IsProduction;
    public static bool IsProduction =>
        Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Production";
}
