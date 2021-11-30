using EventStoreLite;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure.Database;

#nullable enable

namespace Snittlistan.Web.Infrastructure;
public delegate void PublishMessageDelegate(TaskBase task);

public class MessageContext<TTask> : IMessageContext where TTask : TaskBase
{
    public MessageContext(
        TTask task,
        Tenant tenant,
        Guid correlationId,
        Guid causationId)
    {
        Task = task;
        Tenant = tenant;
        CorrelationId = correlationId;
        CausationId = causationId;
    }

    public TTask Task { get; }

    public Tenant Tenant { get; }

    public Guid CorrelationId { get; }

    public Guid CausationId { get; }

    public void PublishMessage(TaskBase task)
    {
        PublishMessageDelegate(task);
    }

    public PublishMessageDelegate PublishMessageDelegate { get; set; } = null!;

    internal async Task ExecuteCommand(
        ICommand command,
        Raven.Client.IDocumentSession documentSession,
        IEventStoreSession eventStoreSession)
    {
        await command.Execute(
            documentSession,
            eventStoreSession,
            t => PublishMessageDelegate(t));
    }
}
