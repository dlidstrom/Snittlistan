#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.TaskHandlers;

public interface ITaskHandler<TTask>
    where TTask : TaskBase
{
    Task Handle(HandlerContext<TTask> context);
}
