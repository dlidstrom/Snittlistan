#nullable enable

namespace Snittlistan.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EventStoreLite;
    using NLog;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Models;
    using Snittlistan.Web.Areas.V2.Tasks;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Database;
    using Snittlistan.Web.Models;

    public abstract class AbstractController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Raven.Client.IDocumentStore DocumentStore { get; set; } = null!;

        public Raven.Client.IDocumentSession DocumentSession { get; set; } = null!;

        public IEventStoreSession EventStoreSession { get; set; } = null!;

        public Databases Databases { get; set; } = null!;

        public EventStore EventStore { get; set; } = null!;

        public TenantConfiguration TenantConfiguration { get; set; } = null!;

        public IMsmqTransaction MsmqTransaction { get; set; } = null!;

        public TaskPublisher TaskPublisher { get; set; } = null!;

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

        protected async Task ExecuteCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            await command.Execute(
                DocumentSession,
                EventStoreSession,
                async task => await TaskPublisher.PublishTask(task, User.Identity.Name));
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

            if (Databases.Snittlistan.ChangeTracker.HasChanges())
            {
                int changes = Databases.Snittlistan.SaveChanges();
                Logger.Info("saved {changes} change(s) to database", changes);
            }
        }
    }
}
