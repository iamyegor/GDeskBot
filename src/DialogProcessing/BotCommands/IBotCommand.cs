using Infrastructure.StateManagement;

namespace DialogProcessing.BotCommands;

public interface IBotCommand
{
    Task<bool> IsApplicable(UserRequest request, CancellationToken cancellationToken);
    Task Execute(UserRequest request, CancellationToken token);
}
