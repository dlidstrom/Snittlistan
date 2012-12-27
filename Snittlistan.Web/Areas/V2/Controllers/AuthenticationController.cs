namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Web;
    using System.Web.Mvc;

    using Raven.Client;

    using Snittlistan.Web.Areas.V2.ViewModels;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Infrastructure.AutoMapper;
    using Snittlistan.Web.Models;
    using Snittlistan.Web.Services;

    public class AuthenticationController : AbstractController
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IDocumentSession session, IAuthenticationService authenticationService)
            : base(session)
        {
            if (authenticationService == null) throw new ArgumentNullException("authenticationService");
            this.authenticationService = authenticationService;
        }

        public ActionResult LogOn()
        {
            return this.View();
        }

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

            return this.RedirectToAction("Index", "Roster");
        }

        public ActionResult LogOff()
        {
            this.authenticationService.SignOut();
            return this.RedirectToAction("Index", "Roster");
        }

        public ActionResult Activate(string id, string activationKey)
        {
            var user = Session.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");
            if (user.ActivationKey != activationKey) throw new InvalidOperationException("Unknown activation key");
            if (user.RequiresPasswordChange)
                return this.View(new SetPasswordViewModel { ActivationKey = activationKey });
            return this.RedirectToAction("Index", "Roster");
        }

        [HttpPost]
        public ActionResult Activate(string id, SetPasswordViewModel vm)
        {
            if (ModelState.IsValid == false) return this.View(vm);
            var user = Session.Load<User>(id);
            if (user == null) throw new HttpException(404, "User not found");
            if (user.ActivationKey != vm.ActivationKey) throw new InvalidOperationException("Unknown activation key");
            if (user.RequiresPasswordChange == false) throw new InvalidOperationException("Password change not allowed");
            user.SetPassword(vm.Password);
            return this.RedirectToAction("Index", "Roster");
        }
    }
}
