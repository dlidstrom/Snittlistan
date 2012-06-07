namespace Snittlistan.Controllers
{
    using System;
    using System.Web.Mvc;
    using Models;
    using Raven.Client;

    public abstract class AbstractController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the AbstractController class.
        /// </summary>
        /// <param name="session">Document session.</param>
        public AbstractController(IDocumentSession session)
        {
            Session = session;
        }

        /// <summary>
        /// Gets the document session.
        /// </summary>
        public new IDocumentSession Session { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // make sure there's an admin user
            using (Session.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(5)))
                if (Session.Load<User>("Admin") == null)
                {
                    // first launch
                    Session.Advanced.DocumentStore.DisableAggressiveCaching();
                    Response.Redirect("/welcome");
                    Response.End();
                }
        }
    }
}