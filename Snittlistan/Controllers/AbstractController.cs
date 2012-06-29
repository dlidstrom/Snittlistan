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
        protected AbstractController(IDocumentSession session)
        {
            Session = session;
        }

        /// <summary>
        /// Gets the document session.
        /// </summary>
        protected new IDocumentSession Session { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // make sure there's an admin user
            if (Session.Load<User>("Admin") == null)
            {
                // first launch
                Response.Redirect("/welcome");
                Response.End();
            }
        }
    }
}