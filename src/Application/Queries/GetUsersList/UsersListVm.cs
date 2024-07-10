using Domain.User;

namespace Application.Queries.GetUsersList;

public class UsersListVm
{
    public IList<User> Users { get; init; } = null!;
}
