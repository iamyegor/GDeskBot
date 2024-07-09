using Domain.Errors;
using Domain.User;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XResults;

namespace Application.Users.Commands;

public record CreateUserCommand(long TelegramId) : IRequest<SuccessOr<Error>>;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, SuccessOr<Error>>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateUserCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SuccessOr<Error>> Handle(CreateUserCommand command, CancellationToken ct)
    {
        User? user = await _dbContext.Users.FirstOrDefaultAsync(
            user => user.TelegramId == command.TelegramId,
            cancellationToken: ct
        );
        if (user != null)
        {
            return Errors.User.AlreadyExists;
        }

        user = new User(command.TelegramId);
        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync(ct);

        return Result.Ok();
    }
}
