using Infrastructure.StateManagement;
using Telegram.Bot;

namespace DialogProcessing.BotCommands;

public class SayHeyCommand : IBotCommand
{
    private readonly TelegramBotClient _client;

    public SayHeyCommand(TelegramBotClient client)
    {
        _client = client;
    }

    public Task<bool> IsApplicable(UserRequest request, CancellationToken cancellationToken)
    {
        bool isApplicable = request.Text.ToLower() == "hey";
        return Task.FromResult(isApplicable);
    }

    public async Task Execute(UserRequest request, CancellationToken token)
    {
        await _client.SendTextMessageAsync(
            request.UserTelegramId,
            "Hey \ud83d\udc4b",
            cancellationToken: token
        );
    }
}
