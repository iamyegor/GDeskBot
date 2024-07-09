using System.Reflection;
using DialogProcessing.BotCommands;
using Microsoft.Extensions.DependencyInjection;

namespace DialogProcessing;

public static class ServiceCollectionExtensions
{
    public static void AddBotCommands(this IServiceCollection services)
    {
        var commandInterface = typeof(IBotCommand);
        var commandTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t =>
                commandInterface.IsAssignableFrom(t)
                && t is { IsInterface: false, IsAbstract: false }
            );

        foreach (var commandType in commandTypes)
        {
            services.AddTransient(commandInterface, commandType);
        }
    }
}
