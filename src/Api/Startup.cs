using System.Text.Json.Serialization;
using Api.Common;
using Api.HostedServices;
using Application;
using DialogProcessing;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
            )
            .AddNewtonsoftJson();

        BotConfiguration botConfig = Configuration
            .GetSection("BotConfiguration")
            .Get<BotConfiguration>();

        services.AddSingleton(botConfig);

        services.AddScoped(_ => new TelegramBotClient(botConfig.Token));
        services.AddApplication();
        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
        services.AddDialogProcessing();
        services.AddInfrastructure(Configuration);

        services.AddHostedService<CreateWebhook>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionsMiddleware>();
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseDeveloperExceptionPage();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
