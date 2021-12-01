using Snittlistan.Queue.Messages;
using Snittlistan.Web.Infrastructure;

#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks;
public interface ITaskHandler<TTask>
    where TTask : TaskBase
{
    Task Handle(MessageContext<TTask> context);
}
