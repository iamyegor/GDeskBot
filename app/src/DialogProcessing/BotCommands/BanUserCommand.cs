using Application.Services;
using DialogProcessing.BotCommands.Common;
using Domain.User;
using Infrastructure.Settings;
using Infrastructure.StateManagement;
using MediatR;
using Telegram.Bot;

namespace DialogProcessing.BotCommands;

public class BanUserCommand : IBotCommand
{
    private readonly IMediator _mediator;
    private readonly TelegramBotClient _client;
    private readonly TopicService _topicService;

    public BanUserCommand(IMediator mediator, TelegramBotClient client, TopicService topicService)
    {
        _mediator = mediator;
        _client = client;
        _topicService = topicService;
    }

    public Task<bool> IsApplicable(UserRequest request, CancellationToken ct)
    {
        bool isGroup = request.Request.Message?.Chat.Id == Settings.GroupId;
        bool hasBanMessage = request.Text == AdminCommandNames.Ban;
        return Task.FromResult(isGroup && hasBanMessage);
    }

    public async Task Execute(UserRequest request, CancellationToken ct)
    {
        int topicId = request.Request.Message!.MessageThreadId!.Value;

        var command = new Application.Commands.BanUserCommand(topicId);
        User user = await _mediator.Send(command, ct);

        await _topicService.CloseTopicAsync(topicId, ct);

        await _client.SendTextMessageAsync(
            user.TelegramId,
            "\ud83d\udeab You have been banned by the admin.\n"
                + "\ud83d\udeab Вы были заблокированы администратором.",
            cancellationToken: ct
        );
    }
}
