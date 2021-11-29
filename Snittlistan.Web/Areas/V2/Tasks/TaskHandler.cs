#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using EventStoreLite;
    using NLog;
    using Postal;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Bits;
    using Snittlistan.Web.Infrastructure.Database;

    public abstract class TaskHandler<TTask>
        : ITaskHandler<TTask>
        where TTask : TaskBase
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public Raven.Client.IDocumentStore DocumentStore { get; set; } = null!;

        public Raven.Client.IDocumentSession DocumentSession { get; set; } = null!;

        public IEventStoreSession EventStoreSession { get; set; } = null!;

        public Databases Databases { get; set; } = null!;

        public IEmailService EmailService { get; set; } = null!;

        public IMsmqTransaction MsmqTransaction { get; set; } = null!;

        public IBitsClient BitsClient { get; set; } = null!;

        public Uri TaskApiUri { get; set; } = null!;

        public TaskPublisher TaskPublisher { get; set; } = null!;

        public abstract Task Handle(MessageContext<TTask> context);

        protected async Task<Tenant> GetCurrentTenant()
        {
            string hostname = CurrentHttpContext.Instance().Request.ServerVariables["SERVER_NAME"];
            Tenant tenant = await Databases.Snittlistan.Tenants.SingleOrDefaultAsync(x => x.Hostname == hostname);
            if (tenant == null)
            {
                throw new Exception($"No tenant found for hostname '{hostname}'");
            }

            return tenant;
        }

        protected async Task ExecuteCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            await command.Execute(
                DocumentSession,
                EventStoreSession,
                async t => await TaskPublisher.PublishTask(t, "system"));
        }

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            return query == null ? throw new ArgumentNullException(nameof(query)) : query.Execute(DocumentSession);
        }
    }
}
