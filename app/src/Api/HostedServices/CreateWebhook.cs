using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Api.HostedServices;

public class CreateWebhook : IHostedService
{
    private readonly BotConfiguration _config;
    private readonly TelegramBotClient _telegramBotClient;
    private readonly ILogger<CreateWebhook> _logger;

    public CreateWebhook(
        BotConfiguration config,
        TelegramBotClient telegramBotClient,
        ILogger<CreateWebhook> logger
    )
    {
        _config = config;
        _telegramBotClient = telegramBotClient;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_config.HostAddress))
        {
            InputFileStream? inputFileStream = null;
            if (EnvironmentResolver.IsProduction)
            {
                string base64EncodedCert = Environment.GetEnvironmentVariable("PUBLIC_CERT")!;
                byte[] certBytes = Convert.FromBase64String(base64EncodedCert);
                MemoryStream stream = new MemoryStream(certBytes);
                inputFileStream = new InputFileStream(stream, "cert.pem");
            }

            await _telegramBotClient.SetWebhookAsync(
                url: $"{_config.HostAddress}/telegram/{_config.WebhookToken}",
                certificate: inputFileStream,
                dropPendingUpdates: true,
                cancellationToken: cancellationToken
            );
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _telegramBotClient.DeleteWebhookAsync(true, cancellationToken);
    }
}
