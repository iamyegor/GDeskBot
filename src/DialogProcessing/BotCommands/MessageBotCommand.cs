using Application.Commands;
using DialogProcessing.BotCommands.Common;
using Infrastructure.Data;
using Infrastructure.Settings;
using Infrastructure.StateManagement;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using User = Domain.User.User;

namespace DialogProcessing.BotCommands;

public class MessageBotCommand : IBotCommand
{
    private readonly ApplicationDbContext _context;
    private readonly TelegramBotClient _client;
    private readonly IMediator _mediator;

    public MessageBotCommand(
        ApplicationDbContext context,
        TelegramBotClient client,
        IMediator mediator
    )
    {
        _context = context;
        _client = client;
        _mediator = mediator;
    }

    public async Task<bool> IsApplicable(UserRequest request, CancellationToken ct)
    {
        // user texts the bot
        bool isTexitingToBot = request.Request.Message?.Chat.Id != Settings.GroupId;

        User? user = await _context.Users.SingleOrDefaultAsync(
            x => x.TelegramId == request.UserTelegramId,
            cancellationToken: ct
        );
        
        return isTexitingToBot && user?.IsBanned == false;
    }

    public async Task Execute(UserRequest request, CancellationToken ct)
    {
        User? user = _context.Users.SingleOrDefault(x => x.TelegramId == request.UserTelegramId);
        if (user == null || user.TopicId == null)
        {
            user = await RegisterTopicAndUser(request, ct);
        }

        try
        {
            await SendMessageToTopic(request, ct, user);
        }
        catch (ApiRequestException ex)
            when (ex.Message.Contains("Bad Request: message thread not found"))
        {
            user = await RegisterTopicAndUser(request, ct);
            await SendMessageToTopic(request, ct, user);
        }
    }

    private async Task<User> RegisterTopicAndUser(UserRequest request, CancellationToken token)
    {
        string name =
            request.Request.Message!.From!.Username ?? request.Request.Message.From.FirstName;

        ForumTopic topic = await _client.CreateForumTopicAsync(
            chatId: Settings.GroupId,
            name: name,
            cancellationToken: token
        );

        RegisterUserWithTopicCommand command = new RegisterUserWithTopicCommand(
            request.UserTelegramId,
            topic.MessageThreadId,
            request.Request.Message.From.Username
        );

        return await _mediator.Send(command, token);
    }

    private async Task SendMessageToTopic(UserRequest request, CancellationToken token, User user)
    {
        await _client.SendTextMessageAsync(
            chatId: Settings.GroupId,
            messageThreadId: user.TopicId,
            text: request.Text,
            cancellationToken: token
        );
    }
}
