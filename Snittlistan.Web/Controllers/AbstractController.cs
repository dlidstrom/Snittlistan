using System;
using System.Web.Mvc;
using EventStoreLite;
using Raven.Client;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.BackgroundTasks;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Controllers
{
    public abstract class AbstractController : Controller
    {
        /// <summary>
        /// Gets the document store.
        /// </summary>
        public IDocumentStore DocumentStore { get; set; }

        /// <summary>
        /// Gets the document session.
        /// </summary>
        public IDocumentSession DocumentSession { get; set; }

        /// <summary>
        /// Gets the event store session.
        /// </summary>
        public IEventStoreSession EventStoreSession { get; set; }

        /// <summary>
        /// Gets the event store.
        /// </summary>
        public EventStore EventStore { get; set; }

        /// <summary>
        /// Gets the tenant configuration.
        /// </summary>
        public TenantConfiguration TenantConfiguration { get; set; }

        protected void SendTask<TTask>(TTask task) where TTask : class
        {
            DocumentSession.Store(BackgroundTask.Create(task, TenantConfiguration));
        }

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            command.Execute(DocumentSession, EventStoreSession);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // load website config to make sure it always migrates
            var websiteContent = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            if (websiteContent == null)
            {
                DocumentSession.Store(new WebsiteConfig(new WebsiteConfig.TeamNameAndLevel[0], false));
            }

            // make sure there's an admin user
            if (DocumentSession.Load<User>("Admin") != null) return;

            // first launch
            Response.Redirect("/v1/welcome");
            Response.End();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction || filterContext.Exception != null) return;

            // this commits the document session
            EventStoreSession.SaveChanges();
        }
    }
}