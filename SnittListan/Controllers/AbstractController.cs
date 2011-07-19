using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Elmah;

namespace SnittListan.Controllers
{
	public class AbstractController : Controller
	{
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

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