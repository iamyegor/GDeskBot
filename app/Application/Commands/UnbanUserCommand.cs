using Domain.User;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.Commands;

public record UnbanUserCommand(string MessageText) : IRequest<Result<User>>;

public class UnbanUserCommandHandler : IRequestHandler<UnbanUserCommand, Result<User>>
{
    private readonly ApplicationDbContext _context;

    public UnbanUserCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<User>> Handle(UnbanUserCommand command, CancellationToken ct)
    {
        string trimmedMessageText = command.MessageText.Trim();
        int indexOfWhiteSpace = trimmedMessageText.IndexOf(' ');
        string telegramUsername = trimmedMessageText.Remove(0, indexOfWhiteSpace + 1);

        User? user = await _context.Users.SingleOrDefaultAsync(
            x => x.TelegramUsername == telegramUsername,
            ct
        );
        if (user == null || !user.IsBanned)
        {
            return Result.Fail("User not found or not banned.");
        }

        user.Unban();
        await _context.SaveChangesAsync(ct);

        return user;
    }
}
