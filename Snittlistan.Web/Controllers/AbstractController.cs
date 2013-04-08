using System.Web.Mvc;
using EventStoreLite;
using Raven.Client;
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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // make sure there's an admin user
            if (this.DocumentSession.Load<User>("Admin") != null) return;

            // first launch
            this.Response.Redirect("/v1/welcome");
            this.Response.End();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction || filterContext.Exception != null) return;

            // this commits the document session
            EventStoreSession.SaveChanges();
        }
    }
}