#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Areas.V2.Tasks;

public interface ITaskHandler<TTask>
    where TTask : TaskBase
{
    Task Handle(MessageContext<TTask> context);
}
