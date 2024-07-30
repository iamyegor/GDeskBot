using Microsoft.Extensions.DependencyInjection;

namespace DialogProcessing;

public static class DependencyInjection
{
    public static IServiceCollection AddDialogProcessing(this IServiceCollection services)
    {
        services.AddSingleton<DialogProcessor>();
        services.AddBotCommands();

        return services;
    }
}
