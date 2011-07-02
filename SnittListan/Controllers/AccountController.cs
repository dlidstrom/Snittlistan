using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Raven.Client;
using SnittListan.Models;
using SnittListan.Services;

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
				if (Membership.ValidateUser(model.UserName, model.Password))
				{
					FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
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
				else
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
			FormsAuthentication.SignOut();

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
			if (ModelState.IsValid)
			{
				// Attempt to register the user
				var user = Session.Query<User>()
					.Where(u => u.Email == model.Email)
					.FirstOrDefault();

				if (user == null)
				{
					var newUser = new User(model.FirstName, model.LastName, model.Email, model.Password);
					newUser.Initialize();
					Session.Store(newUser);
					Session.SaveChanges();

					return RedirectToAction("RegisterSuccess");
				}
				else
				{
					ModelState.AddModelError("Email", "Adressen finns redan.");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		/// <summary>
		/// GET: /Account/ChangePassword
		/// </summary>
		/// <returns></returns>
		[Authorize]
		public ActionResult ChangePassword()
		{
			return View();
		}

		/// <summary>
		/// POST: /Account/ChangePassword
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[Authorize]
		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (ModelState.IsValid)
			{
				// ChangePassword will throw an exception rather
				// than return false in certain failure scenarios.
				bool changePasswordSucceeded;
				try
				{
					MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
					changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
				}
				catch (Exception)
				{
					changePasswordSucceeded = false;
				}

				if (changePasswordSucceeded)
				{
					return RedirectToAction("ChangePasswordSuccess");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "The current password is incorrect or the new password is invalid.");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
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

		#region Status Codes
		private static string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (createStatus)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "User name already exists. Please enter a different user name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
			}
		}
		#endregion
	}
}
