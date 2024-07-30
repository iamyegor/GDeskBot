using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        IHost host = CreateHostBuilder(args)
            .UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration))
            .Build();
        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
