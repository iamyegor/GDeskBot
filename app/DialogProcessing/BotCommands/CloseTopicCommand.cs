using Application.Services;
using DialogProcessing.BotCommands.Common;
using Domain.User;
using Infrastructure.Settings;
using Infrastructure.StateManagement;
using MediatR;
using Telegram.Bot;

namespace DialogProcessing.BotCommands;

public class CloseTopicCommand : IBotCommand
{
    private readonly TelegramBotClient _client;
    private readonly IMediator _mediator;
    private readonly TopicService _topicService;

    public CloseTopicCommand(
        TelegramBotClient client,
        IMediator mediator,
        TopicService topicService
    )
    {
        _client = client;
        _mediator = mediator;
        _topicService = topicService;
    }

    public Task<bool> IsApplicable(UserRequest request, CancellationToken ct)
    {
        bool isGroup = request.Request.Message?.Chat.Id == Settings.GroupId;
        bool closeMessage = request.Text == AdminCommandNames.Close;
        return Task.FromResult(isGroup && closeMessage);
    }

    public async Task Execute(UserRequest request, CancellationToken ct)
    {
        int topicId = request.Request.Message!.MessageThreadId!.Value;
        User user = await _topicService.CloseTopicAsync(topicId, ct);

        await _client.SendTextMessageAsync(
            user.TelegramId,
            "\ud83d\udd12 The thread has been closed by the admin.\n"
                + "\ud83d\udd12 Вопрос был закрыт администратором.",
            cancellationToken: ct
        );
    }
}
