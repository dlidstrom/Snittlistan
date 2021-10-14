#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System.Threading.Tasks;
    using Postal;
    using Raven.Client;

    public interface ITaskHandler<TTask>
    {
        Task Handle(TTask task);
    }

    public abstract class TaskHandler<TTask>
        : ITaskHandler<TTask>
    {
        public IDocumentSession DocumentSession { get; set; } = null!;

        public IEmailService EmailService { get; set; } = null!;

        public abstract Task Handle(TTask task);
    }
}
