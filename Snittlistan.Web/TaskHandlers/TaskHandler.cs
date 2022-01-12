#nullable enable

using NLog;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.TaskHandlers;

public abstract class TaskHandler<TTask>
    : ITaskHandler<TTask>
    where TTask : TaskBase
{
    protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public CompositionRoot CompositionRoot { get; set; } = null!;

    public abstract Task Handle(HandlerContext<TTask> context);

    protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
    {
        return query.Execute(CompositionRoot.DocumentSession);
    }
}
