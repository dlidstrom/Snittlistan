#nullable enable

namespace Snittlistan.Queue.Commands;

public abstract class CommandBase
{
    public CommandBase()
    {
        CorrelationId = Guid.NewGuid();
    }

    public Guid CorrelationId { get; }
}
