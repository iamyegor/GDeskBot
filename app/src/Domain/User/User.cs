using Domain.Common;

namespace Domain.User;

public class User : AggregateRoot<Guid>
{
    public long TelegramId { get; }
    public int? TopicId { get; private set; }
    public bool IsBanned { get; private set; }
    public string? TelegramUsername { get; private set; }

    public User(long telegramId, int? topicId, string? telegramUsername)
        : base(Guid.NewGuid())
    {
        TelegramId = telegramId;
        TopicId = topicId;
        TelegramUsername = telegramUsername;
    }

    private User()
        : base(Guid.NewGuid()) { }

    public void UpdateData(int newTopicId, string? newTelegramUsername)
    {
        TopicId = newTopicId;
        TelegramUsername = newTelegramUsername;
    }

    public void RemoveTopic()
    {
        TopicId = null;
    }

    public void Ban()
    {
        IsBanned = true;
    }

    public void Unban()
    {
        IsBanned = false;
    }
}
