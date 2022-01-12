#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;
using Snittlistan.Web.Infrastructure.Database;

namespace Snittlistan.Web.Infrastructure;

public class MessageContext<TTask> : IPublishContext
    where TTask : TaskBase
{
    private readonly CompositionRoot compositionRoot;

    public MessageContext(
        CompositionRoot compositionRoot,
        TTask task,
        Tenant tenant,
        Guid correlationId,
        Guid causationId)
    {
        this.compositionRoot = compositionRoot;
        Task = task;
        Tenant = tenant;
        CorrelationId = correlationId;
        CausationId = causationId;
    }

    public TTask Task { get; }

    public Tenant Tenant { get; }

    public Guid CorrelationId { get; }

    public Guid CausationId { get; }

    public PublishMessageDelegate PublishMessage { get; set; } = null!;

    public async Task ExecuteCommand(
        CommandBase command)
    {
        CommandExecutor commandExecutor = new(compositionRoot, CausationId, "system");
        await commandExecutor.Execute(command);
    }
}
