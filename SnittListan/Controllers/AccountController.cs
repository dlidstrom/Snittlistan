namespace SnittListan.Controllers
{
	using System;
	using System.Web.Mvc;
	using System.Web.Routing;
	using System.Web.Security;
	using SnittListan.Infrastructure;
	using SnittListan.Models;
	using SnittListan.Services;

	/// <summary>
	/// Controller for the account view.
	/// </summary>
	public class AccountController : Controller
	{
		/// <summary>
		/// Gets or sets the forms authentication service.
		/// </summary>
		public IFormsAuthenticationService FormsService
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the memberships service.
		/// </summary>
		public IMembershipService MembershipService
		{
			get;
			set;
		}

		/// <summary>
		/// Handles the LogOn action.
		/// </summary>
		/// <returns>LogOn view.</returns>
		public ActionResult LogOn()
		{
			return View();
		}

		/// <summary>
		/// Logs on the user then, if successful, redirects to another url.
		/// </summary>
		/// <param name="model">LogOn model.</param>
		/// <param name="returnUrl">Redirect url.</param>
		/// <returns>View after redirect, or same view if something failed.</returns>
		[HttpPost]
		public ActionResult LogOn(LogOnModel model, string returnUrl)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}

			if (returnUrl == null)
			{
				throw new ArgumentNullException("returnUrl");
			}

			if (ModelState.IsValid)
			{
				if (this.MembershipService.ValidateUser(model.UserName, model.Password))
				{
					this.FormsService.SignIn(model.UserName, model.RememberMe);
					if (Url.IsLocalUrl(returnUrl))
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
		/// Handles the LogOff action.
		/// </summary>
		/// <returns>Index of the home view.</returns>
		public ActionResult LogOff()
		{
			this.FormsService.SignOut();

			return RedirectToAction("Index", "Home");
		}

		/// <summary>
		/// Handles the Register GET action.
		/// </summary>
		/// <returns>Register view.</returns>
		public ActionResult Register()
		{
			ViewBag.PasswordLength = this.MembershipService.MinPasswordLength;
			return View();
		}

		/// <summary>
		/// Handles the register POST action.
		/// </summary>
		/// <param name="model">Registration model.</param>
		/// <returns>Index of the Home view, or same page if there was an error.</returns>
		[HttpPost]
		public ActionResult Register(RegisterModel model)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}

			if (ModelState.IsValid)
			{
				// Attempt to register the user
				MembershipCreateStatus createStatus = this.MembershipService.CreateUser(model.UserName, model.Password, model.Email);

				if (createStatus == MembershipCreateStatus.Success)
				{
					this.FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError(string.Empty, AccountValidation.ErrorCodeToString(createStatus));
				}
			}

			// If we got this far, something failed, redisplay form
			ViewBag.PasswordLength = this.MembershipService.MinPasswordLength;
			return View(model);
		}

		/// <summary>
		/// Handles the ChangePassword action.
		/// </summary>
		/// <returns>ChangePassword view.</returns>
		[Authorize]
		public ActionResult ChangePassword()
		{
			ViewBag.PasswordLength = this.MembershipService.MinPasswordLength;
			return View();
		}

		/// <summary>
		/// Handles the ChangePassword POST action.
		/// </summary>
		/// <param name="model">ChangePassword model.</param>
		/// <returns>Redirects to ChangePasswordSuccess, or redisplay if errors.</returns>
		[Authorize]
		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}

			if (ModelState.IsValid)
			{
				if (this.MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
				{
					return RedirectToAction("ChangePasswordSuccess");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "The current password is incorrect or the new password is invalid.");
				}
			}

			// If we got this far, something failed, redisplay form
			ViewBag.PasswordLength = this.MembershipService.MinPasswordLength;
			return View(model);
		}

		/// <summary>
		/// Handles the ChangePasswordSuccess action.
		/// </summary>
		/// <returns>ChangePasswordSuccess view.</returns>
		public ActionResult ChangePasswordSuccess()
		{
			return View();
		}

		/// <summary>
		/// Called when controller is being initialized.
		/// </summary>
		/// <param name="requestContext">Request context.</param>
		protected override void Initialize(RequestContext requestContext)
		{
			if (this.FormsService == null)
			{
				this.FormsService = new FormsAuthenticationService();
			}

			if (this.MembershipService == null)
			{
				this.MembershipService = new AccountMembershipService();
			}

			base.Initialize(requestContext);
		}
	}
}
