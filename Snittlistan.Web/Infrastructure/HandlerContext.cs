#nullable enable

using Snittlistan.Web.Commands;
using Snittlistan.Web.Infrastructure.Database;

namespace Snittlistan.Web.Infrastructure;

public class HandlerContext<TPayload> : IHandlerContext
{
    private readonly CompositionRoot compositionRoot;

    public HandlerContext(
        CompositionRoot compositionRoot,
        TPayload payload,
        Tenant tenant,
        Guid correlationId,
        Guid causationId)
    {
        this.compositionRoot = compositionRoot;
        Payload = payload;
        Tenant = tenant;
        CorrelationId = correlationId;
        CausationId = causationId;
    }

    public TPayload Payload { get; }

    public Tenant Tenant { get; }

    public Guid CorrelationId { get; }

    public Guid CausationId { get; }

    public PublishMessageDelegate PublishMessage { get; set; } = null!;

    public async Task ExecuteCommand(
        CommandBase command)
    {
        CommandExecutor commandExecutor = new(
            compositionRoot,
            CorrelationId,
            CausationId,
            "system");
        await commandExecutor.Execute(command);
    }
}
