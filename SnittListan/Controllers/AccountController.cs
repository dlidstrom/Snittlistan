using System.Linq;
using System.Web;
using System.Web.Mvc;
using Raven.Client;
using SnittListan.Models;
using SnittListan.Services;
using SnittListan.ViewModels;

namespace SnittListan.Controllers
{
	public class AccountController : Controller
	{
		private readonly IFormsAuthenticationService authenticationServce;

		public AccountController(IDocumentSession session, IFormsAuthenticationService authenticationServce)
		{
			this.Session = session;
			this.authenticationServce = authenticationServce;
		}

		public new IDocumentSession Session { get; set; }

		/// <summary>
		/// GET: /Account/LogOn
		/// </summary>
		/// <returns></returns>
		public ActionResult LogOn()
		{
			return View();
		}

		/// <summary>
		/// POST: /Account/LogOn
		/// </summary>
		/// <param name="model"></param>
		/// <param name="returnUrl"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult LogOn(LogOnModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				///if (Membership.ValidateUser(model.UserName, model.Password))
				{
					///FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
					if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
						&& !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
					{
						return Redirect(returnUrl);
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}

				///else
				{
					ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		/// <summary>
		/// GET: /Account/LogOff
		/// </summary>
		/// <returns></returns>
		public ActionResult LogOff()
		{
			///FormsAuthentication.SignOut();

			return RedirectToAction("Index", "Home");
		}

		/// <summary>
		/// GET: /Account/Register
		/// </summary>
		/// <returns></returns>
		public ActionResult Register()
		{
			return View();
		}

		/// <summary>
		/// POST: /Account/Register
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Register(RegisterModel model)
		{
			// attempt to register the user
			var user = Session.Query<User>()
				.Where(u => u.Email == model.Email)
				.FirstOrDefault();

			if (user != null)
				ModelState.AddModelError("Email", "Adressen finns redan.");

			// redisplay form if any errors at this point
			if (!ModelState.IsValid)
				return View(model);

			var newUser = new User(model.FirstName, model.LastName, model.Email, model.Password);
			newUser.Initialize();
			Session.Store(newUser);
			Session.SaveChanges();

			return RedirectToAction("RegisterSuccess");
		}

		/// <summary>
		/// GET: /Account/ChangePassword
		/// </summary>
		/// <returns></returns>
		[Authorize]
		public ActionResult ChangePassword()
		{
			return View(new ChangePasswordViewModel { Email = HttpContext.User.Identity.Name });
		}

		/// <summary>
		/// POST: /Account/ChangePassword
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[Authorize]
		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordViewModel model)
		{
			var user = Session.Query<User>()
				.Where(u => u.Email == model.Email)
				.FirstOrDefault();

			if (user == null)
				return HttpNotFound("Användaren existerar inte.");

			// redisplay form if any errors at this point
			if (!ModelState.IsValid)
				return View(model);

			user.SetPassword(model.NewPassword);
			return RedirectToAction("ChangePasswordSuccess");
		}

		/// <summary>
		/// GET: /Account/ChangePasswordSuccess
		/// </summary>
		/// <returns></returns>
		public ActionResult ChangePasswordSuccess()
		{
			return View();
		}

		/// <summary>
		/// GET: /Account/RegisterSuccess
		/// </summary>
		/// <returns></returns>
		public ActionResult RegisterSuccess()
		{
			return View();
		}
	}
}
