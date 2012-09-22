namespace Snittlistan.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Web.Mvc;

    using MvcContrib;

    using Raven.Client;

    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Models;
    using Snittlistan.Web.Services;
    using Snittlistan.Web.ViewModels.Account;

    /// <summary>
    /// Handles user-actions related to accounts: registering and validating
    /// new users, logging in and logging out.
    /// </summary>
    public class AccountController : AbstractController
    {
        private readonly IAuthenticationService authenticationService;

        /// <summary>
        /// Initializes a new instance of the AccountController class.
        /// </summary>
        /// <param name="session">Document session.</param>
        /// <param name="authenticationService">Authentication service.</param>
        public AccountController(IDocumentSession session, IAuthenticationService authenticationService)
            : base(session)
        {
            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// GET: /Account/LogOn.
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOn()
        {
            return this.View();
        }

        /// <summary>
        /// POST: /Account/LogOn.
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LogOn(LogOnViewModel vm, string returnUrl)
        {
            // find the user in question
            var user = this.Session.FindUserByEmail(vm.Email);

            if (user == null)
            {
                this.ModelState.AddModelError("Email", "Användaren existerar inte.");
            }
            else if (!user.ValidatePassword(vm.Password))
            {
                this.ModelState.AddModelError("Password", "Lösenordet stämmer inte!");
            }
            else if (user.IsActive == false)
            {
                this.ModelState.AddModelError("Inactive", "Användaren har inte aktiverats");
            }

            // redisplay form if any errors at this point
            if (!this.ModelState.IsValid)
                return this.View(vm);

            Debug.Assert(user != null, "user != null");
            this.authenticationService.SetAuthCookie(user.Email, vm.RememberMe);

            if (this.Url.IsLocalUrl(returnUrl)
                && returnUrl.Length > 1
                && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//")
                && !returnUrl.StartsWith("/\\"))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// GET: /Account/LogOff.
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            this.authenticationService.SignOut();
            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// GET: /Account/Register.
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return this.View();
        }

        /// <summary>
        /// POST: /Account/Register.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            // an existing user cannot be registered again
            if (this.Session.FindUserByEmail(model.Email) != null)
                this.ModelState.AddModelError("Email", "Adressen finns redan.");

            // redisplay form if any errors at this point
            if (!this.ModelState.IsValid)
                return this.View(model);

            var newUser = new User(model.FirstName, model.LastName, model.Email, model.Password);
            newUser.Initialize();
            this.Session.Store(newUser);

            return this.RedirectToAction("RegisterSuccess");
        }

        /// <summary>
        /// GET: /Account/ChangePassword.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChangePassword()
        {
            return this.View(new ChangePasswordViewModel { Email = this.HttpContext.User.Identity.Name });
        }

        /// <summary>
        /// POST: /Account/ChangePassword.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            var user = this.Session.FindUserByEmail(model.Email);

            if (user == null)
                this.ModelState.AddModelError("Email", "Användaren existerar inte.");

            // redisplay form if any errors at this point
            if (!this.ModelState.IsValid)
                return this.View(model);

            Debug.Assert(user != null, "user != null");
            user.SetPassword(model.NewPassword);
            return this.RedirectToAction("ChangePasswordSuccess");
        }

        /// <summary>
        /// GET: /Account/ChangePasswordSuccess.
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePasswordSuccess()
        {
            return this.View();
        }

        /// <summary>
        /// GET: /Account/RegisterSuccess.
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterSuccess()
        {
            return this.View();
        }

        /// <summary>
        /// GET: /Account/Verify.
        /// </summary>
        /// <param name="activationKey">Key to activate user.</param>
        /// <returns>LogOn or Register view.</returns>
        public ActionResult Verify(Guid activationKey)
        {
            var user = this.Session.FindUserByActivationKey(activationKey.ToString());

            if (user == null)
                return this.RedirectToAction("Register");

            if (user.IsActive)
                return this.RedirectToAction("LogOn");

            user.Activate();

            return this.RedirectToAction("VerifySuccess");
        }

        /// <summary>
        /// GET: /Account/VerifySuccess.
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifySuccess()
        {
            return this.View();
        }
    }
}