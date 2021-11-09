#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System;
    using System.Threading.Tasks;
    using EventStoreLite;
    using NLog;
    using Postal;
    using Raven.Client;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Queue.Models;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Bits;

    public interface ITaskHandler<TTask>
        where TTask : ITask
    {
        Task Handle(MessageContext<TTask> context);
    }

    public abstract class TaskHandler<TTask>
        : ITaskHandler<TTask>
        where TTask : ITask
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public IDocumentStore DocumentStore { get; set; } = null!;

        public IDocumentSession DocumentSession { get; set; } = null!;

        public IEventStoreSession EventStoreSession { get; set; } = null!;

        public IEmailService EmailService { get; set; } = null!;

        public IMsmqTransaction MsmqTransaction { get; set; } = null!;

        public IBitsClient BitsClient { get; set; } = null!;

        public Uri TaskApiUri { get; set; } = null!;

        public TenantConfiguration TenantConfiguration { get; set; } = null!;

        public TaskPublisher TaskPublisher { get; set; } = null!;

        public abstract Task Handle(MessageContext<TTask> context);

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Execute(
                DocumentSession,
                EventStoreSession,
                t => TaskPublisher.PublishTask(t, "system"));
        }

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            return query == null ? throw new ArgumentNullException(nameof(query)) : query.Execute(DocumentSession);
        }
    }
}
