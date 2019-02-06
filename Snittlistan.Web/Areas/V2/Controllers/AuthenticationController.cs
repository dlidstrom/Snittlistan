namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using Domain;
    using Indexes;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Services;

    public class AuthenticationController : AbstractController
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        public ActionResult LogOnEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOnEmail(EmailViewModel vm, string returnUrl)
        {
            // find the user in question
            var user = DocumentSession.FindUserByEmail(vm.Email);

            if (user == null)
            {
                // locate player
                var player = DocumentSession.Query<Player, PlayerSearch>().FirstOrDefault(x => x.Email == vm.Email);
                if (player == null)
                {
                    ModelState.AddModelError("Email", "Användaren existerar inte.");
                }
                else
                {
                    // TODO: send the email here
                    return View("EmailSent");
                }
            }

            // redisplay form if any errors at this point
            if (!ModelState.IsValid)
                return View(vm);

            return RedirectToAction("LogOnPassword", new { vm.Email, returnUrl });
        }

        public ActionResult LogOnPassword(string email, string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOnPassword(string email, string returnUrl, PasswordViewModel vm)
        {
            // find the user in question
            var user = DocumentSession.FindUserByEmail(email);

            if (!user.ValidatePassword(vm.Password))
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

            return RedirectToAction("Index", "Roster");
        }

        public ActionResult LogOff()
        {
            authenticationService.SignOut();
            return RedirectToAction("Index", "Roster");
        }

        public class EmailViewModel
        {
            [Required(ErrorMessage = "Ange e-postadress")]
            [DataType(DataType.EmailAddress), Display(Name = "E-postadress")]
            public string Email { get; set; }
        }

        public class PasswordViewModel
        {
            [Required(ErrorMessage = "Ange lösenord")]
            [DataType(DataType.Password), Display(Name = "Lösenord")]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }
    }
}