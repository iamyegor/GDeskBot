using Application.Users.Commands;
using Domain.Errors;
using Infrastructure.StateManagement;
using MediatR;
using Telegram.Bot;
using XResults;

namespace DialogProcessing.BotCommands;

public class StartCommand : IBotCommand
{
    private readonly IMediator _mediator;
    private readonly TelegramBotClient _client;

    public StartCommand(IMediator mediator, TelegramBotClient client)
    {
        _mediator = mediator;
        _client = client;
    }

    public Task<bool> IsApplicable(UserRequest request, CancellationToken cancellationToken)
    {
        var commandPayload = request.Text;
        return Task.FromResult(commandPayload.Contains(CommandNames.Start));
    }

    public async Task Execute(UserRequest request, CancellationToken token)
    {
        SuccessOr<Error> result = await _mediator.Send(
            new CreateUserCommand(request.UserTelegramId),
            token
        );

        if (result.IsSuccess)
        {
            await _client.SendTextMessageAsync(
                request.UserTelegramId,
                "You're were successfully registered within the system. \ud83e\udd73",
                cancellationToken: token
            );
        }
        else if (result.Error == Errors.User.AlreadyExists)
        {
            await _client.SendTextMessageAsync(
                request.UserTelegramId,
                "You're already registered in the system, so you don't need to register again",
                cancellationToken: token
            );
        }
    }
}
