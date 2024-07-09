using Application.Common;
using Domain.User;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public class GetUserByTelegramId : IRequest<User?>
{
    public long? TelegramId { get; set; }

    public class Handler : IRequestHandler<GetUserByTelegramId, User?>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // todo: add handler for get user table
        public async Task<User?> Handle(
            GetUserByTelegramId request,
            CancellationToken cancellationToken
        )
        {
            var user = await _dbContext
                .Users.Where(u => u.TelegramId == request.TelegramId)
                .FirstOrDefaultAsync(cancellationToken);
            return user;
        }
    }
}
