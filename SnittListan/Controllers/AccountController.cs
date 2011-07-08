using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib;
using Raven.Client;
using SnittListan.Helpers;
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
			// find the user in question
			var user = Session.FindUserByEmail(model.Email)
				.FirstOrDefault();

			if (user == null)
			{
				ModelState.AddModelError("Email", "Användaren existerar inte.");
			}
			else if (!user.ValidatePassword(model.Password))
			{
				ModelState.AddModelError("Password", "Lösenordet stämmer inte!");
			}
			else if (user.IsActive == false)
			{
				ModelState.AddModelError("Inactive", "Användaren har inte aktiverats");
			}

			// redisplay form if any errors at this point
			if (!ModelState.IsValid)
				return View(model);

			authenticationServce.SetAuthCookie(model.Email, model.RememberMe);

			if (Url.IsLocalUrl(returnUrl)
				&& returnUrl.Length > 1
				&& returnUrl.StartsWith("/")
				&& !returnUrl.StartsWith("//")
				&& !returnUrl.StartsWith("/\\"))
			{
				return Redirect(returnUrl);
			}

			return this.RedirectToAction<HomeController>(c => c.Index());
		}

		/// <summary>
		/// GET: /Account/LogOff
		/// </summary>
		/// <returns></returns>
		public ActionResult LogOff()
		{
			return this.RedirectToAction<HomeController>(c => c.Index());
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
			var user = Session.FindUserByEmail(model.Email)
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

			return this.RedirectToAction(c => c.RegisterSuccess());
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
			var user = Session.FindUserByEmail(model.Email)
				.FirstOrDefault();

			if (user == null)
				return HttpNotFound("Användaren existerar inte.");

			// redisplay form if any errors at this point
			if (!ModelState.IsValid)
				return View(model);

			user.SetPassword(model.NewPassword);
			return this.RedirectToAction(c => c.ChangePasswordSuccess());
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

		/// <summary>
		/// GET: /Account/Verify
		/// </summary>
		/// <param name="activationKey">Key to activate user.</param>
		/// <returns>LogOn or Register view.</returns>
		public ActionResult Verify(Guid activationKey)
		{
			var user = Session.FindUserByActivationKey(activationKey.ToString())
				.FirstOrDefault();

			if (user == null)
				return this.RedirectToAction(c => c.Register());

			if (user.IsActive)
				this.RedirectToAction(c => c.LogOn());

			return this.RedirectToAction(c => c.VerifySuccess());
		}

		/// <summary>
		/// GET: /Account/VerifySuccess
		/// </summary>
		/// <returns></returns>
		public ActionResult VerifySuccess()
		{
			return View();
		}
	}
}
