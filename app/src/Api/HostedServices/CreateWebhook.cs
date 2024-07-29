using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Utils;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Api.HostedServices;

public class CreateWebhook : IHostedService
{
    private readonly BotConfiguration _config;
    private readonly TelegramBotClient _telegramBotClient;

    public CreateWebhook(BotConfiguration config, TelegramBotClient telegramBotClient)
    {
        _config = config;
        _telegramBotClient = telegramBotClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_config.HostAddress))
        {
            InputFileStream? inputFileStream = null;
            if (EnvironmentResolver.IsProduction)
            {
                string certFilePath = "/pub-certs/cert.pem";
                Stream certFileStream = new FileStream(
                    certFilePath,
                    FileMode.Open,
                    FileAccess.Read
                );

                inputFileStream = new InputFileStream(certFileStream);
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
