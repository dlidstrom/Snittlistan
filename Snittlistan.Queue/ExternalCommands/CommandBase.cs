#nullable enable

namespace Snittlistan.Queue.ExternalCommands;

public abstract class CommandBase
{
    public CommandBase()
    {
        CorrelationId = Guid.NewGuid();
    }

    public Guid CorrelationId { get; }
}
