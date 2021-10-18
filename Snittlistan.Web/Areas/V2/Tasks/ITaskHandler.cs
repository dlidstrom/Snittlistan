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
    {
        Task Handle(TTask task);
    }

    public abstract class TaskHandler<TTask>
        : ITaskHandler<TTask>
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

        protected void PublishMessage<TPayload>(TPayload payload)
        {
            MessageEnvelope envelope = new(payload, TaskApiUri);
            MsmqTransaction.PublishMessage(envelope);
        }

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Execute(DocumentSession, EventStoreSession, PublishMessage);
        }

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            return query == null ? throw new ArgumentNullException(nameof(query)) : query.Execute(DocumentSession);
        }

        public abstract Task Handle(TTask task);
    }
}
