namespace Infrastructure.Utils;

public class EnvironmentResolver
{
    public static bool IsDevelopment => !IsProduction;

    public static bool IsProduction =>
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
}
