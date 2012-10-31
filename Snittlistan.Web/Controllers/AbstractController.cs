namespace Snittlistan.Web.Controllers
{
    using System.Web.Mvc;

    using Raven.Client;

    using Snittlistan.Web.Models;

    public abstract class AbstractController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the AbstractController class.
        /// </summary>
        /// <param name="session">Document session.</param>
        protected AbstractController(IDocumentSession session)
        {
            this.Session = session;
        }

        /// <summary>
        /// Gets the document session.
        /// </summary>
        protected new IDocumentSession Session { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // make sure there's an admin user
            if (this.Session.Load<User>("Admin") != null) return;

            // first launch
            this.Response.Redirect("/v1/welcome");
            this.Response.End();
        }
    }
}