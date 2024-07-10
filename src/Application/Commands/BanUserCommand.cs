using Domain.User;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public record BanUserCommand(int TopicId) : IRequest<User>;

public class BanUserCommandHandler : IRequestHandler<BanUserCommand, User>
{
    private readonly ApplicationDbContext _context;

    public BanUserCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(BanUserCommand command, CancellationToken ct)
    {
        User user = await _context.Users.SingleAsync(x => x.TopicId == command.TopicId, ct);
        user.Ban();

        await _context.SaveChangesAsync(ct);

        return user;
    }
}
