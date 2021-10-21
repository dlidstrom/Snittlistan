#nullable enable

namespace Snittlistan.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EventStoreLite;
    using Newtonsoft.Json;
    using NLog;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Queue.Models;
    using Snittlistan.Web.HtmlHelpers;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Database;
    using Snittlistan.Web.Models;

    public abstract class AbstractController : Controller
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly JsonSerializerSettings serializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        protected AbstractController()
        {
            PublishMessage = DefaultPublishMessage;
        }

        public Raven.Client.IDocumentStore DocumentStore { get; set; } = null!;

        public Raven.Client.IDocumentSession DocumentSession { get; set; } = null!;

        public IEventStoreSession EventStoreSession { get; set; } = null!;

        public DatabaseContext Database { get; set; } = null!;

        public EventStore EventStore { get; set; } = null!;

        public TenantConfiguration TenantConfiguration { get; set; } = null!;

        public IMsmqTransaction MsmqTransaction { get; set; } = null!;

        protected new CustomPrincipal User => (CustomPrincipal)HttpContext.User;

        protected Action<object> PublishMessage { get; private set; }

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Execute(DocumentSession, EventStoreSession, PublishMessage);
        }

        protected async Task PublishDelayedTaskAsync(ITask task, TimeSpan sendAfter)
        {
            string businessKey = task.BusinessKey.ToString();
            IList<DelayedTask> existingTasks = await Database.DelayedTasks
                .Where(x => x.BusinessKey == businessKey && x.PublishedDate == null)
                .ToListAsync();
            if (existingTasks.Any())
            {
                return;
            }

            DelayedTask delayedTask = Database.DelayedTasks.Add(
                new(businessKey, task.ToJson(serializerSettings), DateTime.Now.Add(sendAfter)));
            Logger.Info("added delayed task: {@delayedTask}", delayedTask);
        }

        protected void DefaultPublishMessage<TPayload>(TPayload payload)
        {
            string routeUrl = Url.HttpRouteUrl("DefaultApi", new { controller = "Task" });
            Debug.Assert(Request.Url != null, "Request.Url != null");
            string uriString = $"{Request.Url?.Scheme}://{Request.Url?.Host}:{Request.Url?.Port}{routeUrl}";
            Uri? uri = new(uriString);
            MessageEnvelope envelope = new(payload, uri);
            MsmqTransaction.PublishMessage(envelope);
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
