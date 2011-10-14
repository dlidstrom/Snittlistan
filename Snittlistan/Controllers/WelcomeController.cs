using System.Configuration;
using System.Web.Mvc;
using MvcContrib;
using Raven.Client;
using Snittlistan.Models;
using Snittlistan.ViewModels;

namespace Snittlistan.Controllers
{
	public class WelcomeController : AbstractController
	{
		public WelcomeController(IDocumentSession session)
			: base(session)
		{ }

		public ActionResult Index()
		{
			AssertAdminUserExists();

			string email = "admin@" + ConfigurationManager.AppSettings["Domain"];
			var vm = new RegisterViewModel
			{
				Email = email,
				ConfirmEmail = email
			};

			return View(vm);
		}

		[HttpPost]
		public ActionResult CreateAdmin(RegisterViewModel adminUser)
		{
			AssertAdminUserExists();

			if (!ModelState.IsValid)
				return View("Index");

			var user = new User(
				adminUser.FirstName,
				adminUser.LastName,
				adminUser.Email,
				adminUser.Email)
				{
					Id = "Admin"
				};
			user.Activate();
			Session.Store(user);

			return this.RedirectToAction(c => c.Success());
		}

		public ActionResult Success()
		{
			return View();
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			// don't load the non-existing admin user
		}

		private void AssertAdminUserExists()
		{
			if (Session.Load<User>("Admin") != null)
			{
				Response.Redirect("/");
				Response.End();
			}
		}
	}
}
