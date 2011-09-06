using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Elmah;
using Raven.Client;

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
		/// Gets or sets the document session.
		/// </summary>
		public new IDocumentSession Session { get; private set; }

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			if (HttpContext.Request.UserLanguages == null)
				return;

			// try to set culture
			foreach (var lang in HttpContext.Request.UserLanguages)
			{
				try
				{
					var ci = new CultureInfo(lang);
					Thread.CurrentThread.CurrentCulture = ci;
					Thread.CurrentThread.CurrentUICulture = ci;
					break;
				}
				catch (Exception ex)
				{
					ErrorSignal
						.FromCurrentContext()
						.Raise(ex);
				}
			}
		}
	}
}