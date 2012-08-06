namespace Snittlistan.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Helpers;
    using Models;
    using MvcContrib;
    using Raven.Client;
    using Services;
    using ViewModels.Account;

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
            return View();
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
            var user = Session.FindUserByEmail(vm.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Användaren existerar inte.");
            }
            else if (!user.ValidatePassword(vm.Password))
            {
                ModelState.AddModelError("Password", "Lösenordet stämmer inte!");
            }
            else if (user.IsActive == false)
            {
                ModelState.AddModelError("Inactive", "Användaren har inte aktiverats");
            }

            // redisplay form if any errors at this point
            if (!ModelState.IsValid)
                return View(vm);

            Debug.Assert(user != null, "user != null");
            authenticationService.SetAuthCookie(user.Email, vm.RememberMe);

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
        /// GET: /Account/LogOff.
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            authenticationService.SignOut();
            return this.RedirectToAction<HomeController>(c => c.Index());
        }

        /// <summary>
        /// GET: /Account/Register.
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
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
            if (Session.FindUserByEmail(model.Email) != null)
                ModelState.AddModelError("Email", "Adressen finns redan.");

            // redisplay form if any errors at this point
            if (!ModelState.IsValid)
                return View(model);

            var newUser = new User(model.FirstName, model.LastName, model.Email, model.Password);
            newUser.Initialize();
            Session.Store(newUser);

            return this.RedirectToAction(c => c.RegisterSuccess());
        }

        /// <summary>
        /// GET: /Account/ChangePassword.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel { Email = HttpContext.User.Identity.Name });
        }

        /// <summary>
        /// POST: /Account/ChangePassword.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Authorize]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            var user = Session.FindUserByEmail(model.Email);

            if (user == null)
                ModelState.AddModelError("Email", "Användaren existerar inte.");

            // redisplay form if any errors at this point
            if (!ModelState.IsValid)
                return View(model);

            user.SetPassword(model.NewPassword);
            return this.RedirectToAction(c => c.ChangePasswordSuccess());
        }

        /// <summary>
        /// GET: /Account/ChangePasswordSuccess.
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        /// <summary>
        /// GET: /Account/RegisterSuccess.
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterSuccess()
        {
            return View();
        }

        /// <summary>
        /// GET: /Account/Verify.
        /// </summary>
        /// <param name="activationKey">Key to activate user.</param>
        /// <returns>LogOn or Register view.</returns>
        public ActionResult Verify(Guid activationKey)
        {
            var user = Session.FindUserByActivationKey(activationKey.ToString());

            if (user == null)
                return this.RedirectToAction(c => c.Register());

            if (user.IsActive)
                return this.RedirectToAction(c => c.LogOn());

            user.Activate();

            return this.RedirectToAction(c => c.VerifySuccess());
        }

        /// <summary>
        /// GET: /Account/VerifySuccess.
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifySuccess()
        {
            return View();
        }
    }
}