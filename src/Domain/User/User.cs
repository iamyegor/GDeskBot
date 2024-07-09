using Domain.Common;

namespace Domain.User;

public class User : AggregateRoot<Guid>
{
    public long TelegramId { get; }

    public User(long telegramId)
        : base(Guid.NewGuid())
    {
        TelegramId = telegramId;
    }

    public User()
        : base(Guid.NewGuid()) { }
}
