namespace SnittListan.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;

	/// <summary>
	/// Controller for the Home view.
	/// </summary>
	public class HomeController : Controller
	{
		/// <summary>
		/// Handles the Index action.
		/// </summary>
		/// <returns>Home view.</returns>
		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to ASP.NET MVC!";

			return View();
		}

		/// <summary>
		/// Handles the About action.
		/// </summary>
		/// <returns>About view.</returns>
		public ActionResult About()
		{
			return View();
		}
	}
}
