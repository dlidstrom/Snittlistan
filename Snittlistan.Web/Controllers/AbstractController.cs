#nullable enable

namespace Snittlistan.Web.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using EventStoreLite;
    using NLog;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Queue.Models;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Database;
    using Snittlistan.Web.Models;

    public abstract class AbstractController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Raven.Client.IDocumentStore DocumentStore { get; set; } = null!;

        public Raven.Client.IDocumentSession DocumentSession { get; set; } = null!;

        public IEventStoreSession EventStoreSession { get; set; } = null!;

        public DatabaseContext Database { get; set; } = null!;

        public EventStore EventStore { get; set; } = null!;

        public TenantConfiguration TenantConfiguration { get; set; } = null!;

        public IMsmqTransaction MsmqTransaction { get; set; } = null!;

        protected new CustomPrincipal User => (CustomPrincipal)HttpContext.User;

        protected Guid CorrelationId
        {
            get
            {
                if (HttpContext.Items["CorrelationId"] is Guid correlationId)
                {
                    return correlationId;
                }

                correlationId = Guid.NewGuid();
                HttpContext.Items["CorrelationId"] = correlationId;
                return correlationId;
            }
        }

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Execute(DocumentSession, EventStoreSession, PublishTask);
        }

        // TODO Use TaskPublisher
        protected void PublishTask(ITask task)
        {
            DoPublishDelayedTask(task, DateTime.MinValue);
        }

        protected void PublishDelayedTask(ITask task, TimeSpan sendAfter)
        {
            DateTime publishDate = DateTime.Now.Add(sendAfter);
            DoPublishDelayedTask(task, publishDate);
        }

        private void DoPublishDelayedTask(ITask task, DateTime publishDate)
        {
            string businessKey = task.BusinessKey.ToString();
            DelayedTask delayedTask = Database.DelayedTasks.Add(new(
                task,
                publishDate,
                TenantConfiguration.TenantId,
                CorrelationId,
                null,
                Guid.NewGuid()));
            Logger.Info("added delayed task: {@delayedTask}", delayedTask);
            HostingEnvironment.QueueBackgroundWorkItem(async ct => await PublishMessage(businessKey, ct));

            async Task PublishMessage(string businessKey, CancellationToken ct)
            {
                try
                {
                    using MsmqGateway.MsmqTransactionScope scope = MsmqGateway.AutoCommitScope();
                    using DatabaseContext context = new();
                    DelayedTask delayedTask = await context.DelayedTasks.SingleOrDefaultAsync(x => x.BusinessKeyColumn == businessKey, ct);
                    MessageEnvelope message = new(
                        delayedTask.Task,
                        delayedTask.TenantId,
                        delayedTask.CorrelationId,
                        delayedTask.CausationId,
                        delayedTask.MessageId);
                    scope.PublishMessage(message);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "failed to publish message");
                }
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // load website config to make sure it always migrates
            WebsiteConfig websiteContent = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            if (websiteContent == null)
            {
                DocumentSession.Store(new WebsiteConfig(new WebsiteConfig.TeamNameAndLevel[0], false, -1, 2019));
            }

            // make sure there's an admin user
            if (DocumentSession.Load<User>(Models.User.AdminId) != null)
            {
                return;
            }

            // first launch
            Response.Redirect("/v1/welcome");
            Response.End();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction || filterContext.Exception != null)
            {
                return;
            }

            MsmqTransaction.Commit();

            // this commits the document session
            EventStoreSession.SaveChanges();

            if (Database.ChangeTracker.HasChanges())
            {
                int changes = Database.SaveChanges();
                Logger.Info("saved {changes} change(s) to database", changes);
            }
        }
    }
}
