using Infrastructure.StateManagement;

namespace DialogProcessing.BotCommands.Common;

public interface IBotCommand
{
    Task<bool> IsApplicable(UserRequest request, CancellationToken ct);
    Task Execute(UserRequest request, CancellationToken ct);
}
