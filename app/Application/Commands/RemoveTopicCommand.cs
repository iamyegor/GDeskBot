using Domain.User;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands;

public record RemoveTopicCommand(int MessageThreadId) : IRequest<User>;

public class RemoveTopicCommandHandler : IRequestHandler<RemoveTopicCommand, User>
{
    private readonly ApplicationDbContext _context;

    public RemoveTopicCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(RemoveTopicCommand command, CancellationToken ct)
    {
        User user = await _context.Users.SingleAsync(x => x.TopicId == command.MessageThreadId, ct);

        user.RemoveTopic();
        await _context.SaveChangesAsync(ct);

        return user;
    }
}
