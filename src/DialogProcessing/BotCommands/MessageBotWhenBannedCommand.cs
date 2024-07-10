using DialogProcessing.BotCommands.Common;
using Domain.User;
using Infrastructure.Data;
using Infrastructure.Settings;
using Infrastructure.StateManagement;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace DialogProcessing.BotCommands;

public class MessageBotWhenBannedCommand : IBotCommand
{
    private readonly ApplicationDbContext _context;
    private readonly TelegramBotClient _client;

    public MessageBotWhenBannedCommand(ApplicationDbContext context, TelegramBotClient client)
    {
        _context = context;
        _client = client;
    }

    public async Task<bool> IsApplicable(UserRequest request, CancellationToken ct)
    {
        // user texts the bot
        bool isTexitingToBot = request.Request.Message?.Chat.Id != Settings.GroupId;

        User? user = await _context.Users.SingleOrDefaultAsync(
            x => x.TelegramId == request.UserTelegramId,
            cancellationToken: ct
        );

        return isTexitingToBot && user?.IsBanned == true;
    }

    public async Task Execute(UserRequest request, CancellationToken ct)
    {
        await _client.SendTextMessageAsync(
            chatId: request.UserTelegramId,
            text: "\ud83d\udeab You have been banned. You can't send messages to the bot.\n"
                + "\ud83d\udeab Вы были заблокированы. Вы не можете отправлять сообщения боту.",
            cancellationToken: ct
        );
    }
}
