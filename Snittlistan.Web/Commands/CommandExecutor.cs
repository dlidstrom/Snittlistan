#nullable enable

using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;
using System.Reflection;

namespace Snittlistan.Web.Commands;

public class CommandExecutor
{
    private readonly CompositionRoot compositionRoot;
    private readonly Guid correlationId;
    private readonly Guid? causationId;
    private readonly string createdBy;

    public CommandExecutor(
        CompositionRoot compositionRoot,
        Guid correlationId,
        Guid? causationId,
        string createdBy)
    {
        this.compositionRoot = compositionRoot;
        this.correlationId = correlationId;
        this.causationId = causationId;
        this.createdBy = createdBy;
    }

    public async Task Execute<TCommand>(TCommand command)
        where TCommand : class
    {
        Type handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        MethodInfo handleMethod = handlerType.GetMethod(nameof(ICommandHandler<TCommand>.Handle));
        object handler = compositionRoot.Kernel.Resolve(handlerType);
        Tenant tenant = await compositionRoot.GetCurrentTenant();
        TaskPublisher taskPublisher = new(
            tenant,
            compositionRoot.Databases,
            correlationId,
            causationId);
        IHandlerContext publishContext = (IHandlerContext)Activator.CreateInstance(
            typeof(HandlerContext<>).MakeGenericType(command.GetType()),
            command,
            tenant,
            correlationId,
            causationId);
        publishContext.PublishMessage = (task, publishDate) =>
        {
            if (publishDate != null)
            {
                taskPublisher.PublishDelayedTask(task, publishDate.Value, createdBy);
            }
            else
            {
                taskPublisher.PublishTask(task, createdBy);
            }
        };
        Task task = (Task)handleMethod.Invoke(handler, new[] { publishContext });
        await task;
    }
}
