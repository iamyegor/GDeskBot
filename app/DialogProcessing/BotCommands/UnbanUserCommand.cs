using DialogProcessing.BotCommands.Common;
using Domain.User;
using Infrastructure.Settings;
using Infrastructure.StateManagement;
using MediatR;
using Telegram.Bot;
using XResults;

namespace DialogProcessing.BotCommands;

public class UnbanUserCommand : IBotCommand
{
    private readonly TelegramBotClient _client;
    private readonly IMediator _mediator;

    public UnbanUserCommand(IMediator mediator, TelegramBotClient client)
    {
        _mediator = mediator;
        _client = client;
    }

    public Task<bool> IsApplicable(UserRequest request, CancellationToken ct)
    {
        bool isGroup = request.Request.Message?.Chat.Id == Settings.GroupId;
        bool unbanMessage = request.Text.Trim().StartsWith(AdminCommandNames.Unban + " ");
        return Task.FromResult(isGroup && unbanMessage);
    }

    public async Task Execute(UserRequest request, CancellationToken ct)
    {
        Application.Commands.UnbanUserCommand command = new Application.Commands.UnbanUserCommand(
            request.Text
        );
        Result<User> userOrError = await _mediator.Send(command, ct);

        if (userOrError.IsSuccess)
        {
            await _client.SendTextMessageAsync(
                request.Request.Message!.Chat.Id,
                "\ud83d\udfe2 The user has been unbanned.\n"
                    + "\ud83d\udfe2 Пользователь был разблокирован.",
                cancellationToken: ct
            );

            await _client.SendTextMessageAsync(
                userOrError.Value.TelegramId,
                "\ud83d\udca5 You has been unbanned by the admin.\n"
                    + "\ud83d\udca5 Вы были разблокированы администратором.",
                cancellationToken: ct
            );
        }
        else
        {
            await _client.SendTextMessageAsync(
                request.Request.Message!.Chat.Id,
                "\ud83e\udd37\u200d\u2640\ufe0f The user you were trying to unban either doesn't exist or is not banned.\n"
                    + "\ud83e\udd37\u200d\u2640\ufe0f Пользователь, которого вы пытались разблокировать, либо не существует, либо не заблокирован.",
                cancellationToken: ct
            );
        }
    }
}
