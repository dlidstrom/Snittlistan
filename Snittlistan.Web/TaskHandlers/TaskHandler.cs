#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Commands;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.TaskHandlers;

public abstract class TaskHandler<TTask, TCommand>
    : ITaskHandler<TTask>
    where TTask : TaskBase
    where TCommand : class
{
    public CompositionRoot CompositionRoot { get; set; } = null!;

    public async Task Handle(HandlerContext<TTask> context)
    {
        TCommand command = CreateCommand(context.Payload);
        CommandExecutor commandExecutor = new(
            CompositionRoot,
            context.CorrelationId,
            context.CausationId,
            "system");
        await commandExecutor.Execute(command);
    }

    protected abstract TCommand CreateCommand(TTask payload);
}
