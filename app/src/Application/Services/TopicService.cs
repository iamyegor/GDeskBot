using Application.Commands;
using Domain.User;
using Infrastructure.Settings;
using MediatR;
using Telegram.Bot;

namespace Application.Services;

public class TopicService
{
    private readonly IMediator _mediator;
    private readonly TelegramBotClient _client;

    public TopicService(IMediator mediator, TelegramBotClient client)
    {
        _mediator = mediator;
        _client = client;
    }

    public async Task<User> CloseTopicAsync(int topicId, CancellationToken ct)
    {
        var command = new RemoveTopicCommand(topicId);
        User user = await _mediator.Send(command, ct);

        await _client.DeleteForumTopicAsync(Settings.GroupId, topicId, cancellationToken: ct);

        return user;
    }
}
