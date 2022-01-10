#nullable enable

namespace Snittlistan.Web.Commands;

public abstract class CommandBase
{
    public CommandBase()
    {
        CorrelationId = Guid.NewGuid();
    }

    public Guid CorrelationId { get; }
}
