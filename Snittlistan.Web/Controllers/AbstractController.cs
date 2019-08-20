namespace Snittlistan.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Web.Mvc;
    using EventStoreLite;
    using Raven.Client;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Queue.Models;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Models;

    public abstract class AbstractController : Controller
    {
        public Action<object> PublishMessage;

        protected AbstractController()
        {
            PublishMessage = DefaultPublishMessage;
        }

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

        /// <summary>
        /// Gets the msmq transaction.
        /// </summary>
        public IMsmqTransaction MsmqTransaction { get; set; }

        protected new CustomPrincipal User => HttpContext.User as CustomPrincipal;

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            command.Execute(DocumentSession, EventStoreSession, PublishMessage);
        }

        protected void DefaultPublishMessage<TPayload>(TPayload payload)
        {
            string routeUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Task" });
            Debug.Assert(Request.Url != null, "Request.Url != null");
            string uriString = $"{Request.Url.Scheme}://{Request.Url.Host}:{Request.Url.Port}{routeUrl}";
            var uri = new Uri(uriString);
            var envelope = new MessageEnvelope(payload, uri);
            MsmqTransaction.PublishMessage(envelope);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // load website config to make sure it always migrates
            WebsiteConfig websiteContent = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            if (websiteContent == null)
            {
                var config =
                    new WebsiteConfig(
                        new WebsiteConfig.TeamNameAndLevel[0],
                        false,
                        false,
                        -1,
                        2020);
                DocumentSession.Store(config);
            }

            // make sure there's an admin user
            if (DocumentSession.Load<User>(Models.User.AdminId) != null) return;

            // first launch
            Response.Redirect("/v1/welcome");
            Response.End();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction || filterContext.Exception != null) return;

            MsmqTransaction.Commit();

            // this commits the document session
            EventStoreSession.SaveChanges();
        }
    }
}
