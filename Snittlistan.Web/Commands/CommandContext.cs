#nullable enable

using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;

namespace Snittlistan.Web.Commands;

public class CommandContext<TCommand> : IPublishContext
    where TCommand : CommandBase
{
    public CommandContext(
        TCommand command,
        Tenant tenant,
        Guid correlationId,
        Guid causationId)
    {
        Command = command;
        Tenant = tenant;
        CorrelationId = correlationId;
        CausationId = causationId;
    }

    public TCommand Command { get; }

    public Tenant Tenant { get; }

    public Guid CorrelationId { get; }

    public Guid CausationId { get; }

    public PublishMessageDelegate PublishMessage { get; set; } = null!;
}
