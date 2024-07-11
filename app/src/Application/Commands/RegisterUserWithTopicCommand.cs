using Domain.Errors;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;
using User = Domain.User.User;

namespace Application.Commands;

public record RegisterUserWithTopicCommand(
    long UserTelegramId,
    int TopicId,
    string? TelegramUsername
) : IRequest<Result<User, Error>>;

public class RegisterUserWithTopicCommandHandler
    : IRequestHandler<RegisterUserWithTopicCommand, Result<User, Error>>
{
    private readonly ApplicationDbContext _context;

    public RegisterUserWithTopicCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<User, Error>> Handle(
        RegisterUserWithTopicCommand command,
        CancellationToken ct
    )
    {
        User? user = await _context.Users.SingleOrDefaultAsync(
            x => x.TelegramId == command.UserTelegramId,
            ct
        );

        if (user != null)
        {
            user.UpdateData(command.TopicId, command.TelegramUsername);
        }
        else
        {
            user = new User(command.UserTelegramId, command.TopicId, command.TelegramUsername);
            _context.Users.Add(user);
        }

        await _context.SaveChangesAsync(ct);
        return user;
    }
}
