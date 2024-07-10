using DialogProcessing.BotCommands.Common;
using Infrastructure.Data;
using Infrastructure.Settings;
using Infrastructure.StateManagement;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using User = Domain.User.User;

namespace DialogProcessing.BotCommands;

public class AnswerUserCommand : IBotCommand
{
    private readonly TelegramBotClient _client;
    private readonly ApplicationDbContext _context;

    public AnswerUserCommand(TelegramBotClient client, ApplicationDbContext context)
    {
        _client = client;
        _context = context;
    }

    public Task<bool> IsApplicable(UserRequest request, CancellationToken ct)
    {
        bool isGroup = request.Request.Message?.Chat.Id == Settings.GroupId;
        return Task.FromResult(isGroup && !AdminCommandNames.IsAdminCommand(request.Text));
    }

    public async Task Execute(UserRequest request, CancellationToken ct)
    {
        User user = await _context.Users.SingleAsync(
            x => x.TopicId == request.Request.Message!.MessageThreadId,
            cancellationToken: ct
        );

        await _client.SendTextMessageAsync(user.TelegramId, request.Text, cancellationToken: ct);
    }
}
