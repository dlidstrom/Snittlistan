#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Infrastructure;

    public interface ITaskHandler<TTask>
        where TTask : TaskBase
    {
        Task Handle(MessageContext<TTask> context);
    }
}
