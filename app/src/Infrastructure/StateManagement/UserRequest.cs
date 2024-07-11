using Telegram.Bot.Types;

namespace Infrastructure.StateManagement;

public class UserRequest
{
    public int MessageId { get; }
    public long UserTelegramId { get; }
    public string Text { get; }
    public CallbackQuery? CallbackQuery { get; }
    public Update Request { get; }

    public UserRequest(Update request)
    {
        UserTelegramId =
            request.Message?.From?.Id
            ?? request.CallbackQuery?.From.Id
            ?? throw new ArgumentException();
        MessageId =
            request.Message?.MessageId
            ?? request.CallbackQuery?.Message?.MessageId
            ?? throw new ArgumentException();
        Text =
            request.Message?.Text ?? request.CallbackQuery?.Data ?? throw new ArgumentException();
        CallbackQuery = request.CallbackQuery;
        Request = request;
    }
}
