using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Elmah;
using Raven.Client;
using Snittlistan.Models;

namespace Snittlistan.Controllers
{
	public class AbstractController : Controller
	{
		/// <summary>
		/// Initializes a new instance of the AbstractController class.
		/// </summary>
		public AbstractController()
		{ }

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
			if (Session.Load<User>("Admin") == null)
			{
				// first launch
				Response.Redirect("/welcome");
				Response.End();
			}
		}
	}
}