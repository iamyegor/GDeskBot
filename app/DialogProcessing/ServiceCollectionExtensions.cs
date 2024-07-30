using System.Reflection;
using DialogProcessing.BotCommands.Common;
using Microsoft.Extensions.DependencyInjection;

namespace DialogProcessing;

public static class ServiceCollectionExtensions
{
    public static void AddBotCommands(this IServiceCollection services)
    {
        Type commandInterface = typeof(IBotCommand);
        IEnumerable<Type> commandTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t =>
                commandInterface.IsAssignableFrom(t)
                && t is { IsInterface: false, IsAbstract: false }
            );

        foreach (Type commandType in commandTypes)
        {
            services.AddTransient(commandInterface, commandType);
        }
    }
}
