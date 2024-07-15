using System;
using System.Threading;
using System.Threading.Tasks;
using DialogProcessing;
using Infrastructure.StateManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TelegramController : Controller
{
    private readonly BotConfiguration _configuration;
    private readonly TelegramBotClient _telegramBotClient;
    private readonly ILogger<TelegramController> _logger;
    private readonly DialogProcessor _dialogProcessor;

    public TelegramController(
        BotConfiguration configuration,
        TelegramBotClient telegramBotClient,
        DialogProcessor dialogProcessor,
        ILogger<TelegramController> logger)
    {
        _configuration = configuration;
        _telegramBotClient = telegramBotClient;
        _dialogProcessor = dialogProcessor;
        _logger = logger;
    }

    [HttpPost("{token?}")]
    public async Task Webhook(
        string token,
        [FromBody] Update request,
        CancellationToken cancellationToken
    )
    {
        if (token != _configuration.WebhookToken)
        {
            return;
        }

        if (request.Message?.Text == null)
        {
            return;
        }

        UserRequest userRequest = new UserRequest(request);

        try
        {
            await _dialogProcessor.ProcessCommand(userRequest, cancellationToken);
        }
        catch (Exception e)
        {
            await _telegramBotClient.SendTextMessageAsync(
                userRequest.UserTelegramId,
                "Sorry, can not process your request 😞",
                cancellationToken: cancellationToken
            );
            _logger.LogInformation(
                e,
                "Exception while processing request from user: {User} with command {Command}",
                userRequest.UserTelegramId,
                userRequest.Text
            );
        }
    }
}
