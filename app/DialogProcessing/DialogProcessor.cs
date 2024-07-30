using DialogProcessing.BotCommands.Common;
using Infrastructure.StateManagement;

namespace DialogProcessing;

public class DialogProcessor
{
    private readonly List<IBotCommand> _commands;

    public DialogProcessor(IEnumerable<IBotCommand> processorsList)
    {
        _commands = processorsList.ToList();
    }

    public async Task ProcessCommand(UserRequest request, CancellationToken token)
    {
        foreach (IBotCommand command in _commands)
        {
            if (await command.IsApplicable(request, token))
            {
                await command.Execute(request, token);
                return;
            }
        }
    }
}
