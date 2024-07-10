using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.GetUsersList;

public class GetUsersListQuery : IRequest<UsersListVm>
{
    public class Handler : IRequestHandler<GetUsersListQuery, UsersListVm>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UsersListVm> Handle(
            GetUsersListQuery request,
            CancellationToken cancellationToken
        )
        {
            var response = new UsersListVm
            {
                Users = await _dbContext.Users.ToListAsync(cancellationToken)
            };
            return response;
        }
    }
}
