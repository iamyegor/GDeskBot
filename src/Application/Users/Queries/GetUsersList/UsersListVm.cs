using Domain.User;

namespace Application.Users.Queries.GetUsersList;

public class UsersListVm
{
    public IList<User> Users { get; init; } = null!;
}
