namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Domain;
    using Indexes;
    using Queue.Messages;
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

        public ActionResult LogOn()
        {
            return RedirectToActionPermanent("LogOnEmail");
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
                Player[] players;
                if (vm.PlayerId != null)
                {
                    players = new[]
                    {
                        DocumentSession.Load<Player>(vm.PlayerId)
                    };
                }
                else
                {
                    players = DocumentSession.Query<Player, PlayerSearch>().Where(x => x.Email == vm.Email).ToArray();
                }

                if (players.Length == 0)
                {
                    ModelState.AddModelError("Email", "Spelare med den e-postadressen finns inte.");
                }
                else if (players.Length == 1)
                {
                    var player = players[0];
                    var token = new OneTimeToken();
                    Debug.Assert(Request.Url != null, "Request.Url != null");
                    token.Activate(
                        oneTimeKey =>
                        {
                            var activationUri =
                                Url.Action(
                                    "OneTimeTokenLogOn",
                                    "Authentication",
                                    new
                                    {
                                        oneTimeKey
                                    },
                                    Request.Url.Scheme);
                            PublishMessage(new OneTimeKeyEvent(player.Email, activationUri));
                        },
                        player.Id);
                    DocumentSession.Store(token);
                    return View("EmailSent");
                }
                else if (players.Length > 1)
                {
                    ViewBag.PlayerId = DocumentSession.CreatePlayerSelectList(
                        getPlayers: () => players,
                        textFormatter: p => $"{p.Name} ({p.Nickname})");
                    return View();
                }
                else
                {
                    throw new Exception("Unhandled case");
                }
            }

            // redisplay form if any errors at this point
            if (!ModelState.IsValid)
                return View(vm);

            return RedirectToAction("LogOnPassword", new { vm.Email, returnUrl });
        }

        public ActionResult LogOnPassword(string email, string returnUrl)
        {
            return View(new PasswordViewModel { Email = email, RememberMe = true });
        }

        [HttpPost]
        public ActionResult LogOnPassword(string returnUrl, PasswordViewModel vm)
        {
            // find the user in question
            var user = DocumentSession.FindUserByEmail(vm.Email);

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

        public ActionResult OneTimeTokenLogOn(string oneTimeKey)
        {
            var oneTimeToken = DocumentSession.Query<OneTimeToken, OneTimeTokenIndex>().Single(x => x.OneTimeKey == oneTimeKey);
            if (DocumentSession.Load<Player>(oneTimeToken.Payload) == null)
                throw new HttpException(404, "Player not found");
            oneTimeToken.ApplyToken(
                () => authenticationService.SetAuthCookie(oneTimeToken.Payload, true));
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
            [DataType(DataType.EmailAddress)]
            [Display(Name = "E-postadress")]
            public string Email { get; set; }

            public string PlayerId { get; set; }
        }

        public class PasswordViewModel
        {
            [Required(ErrorMessage = "Ange e-postadress")]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "E-postadress")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Ange lösenord")]
            [DataType(DataType.Password)]
            [Display(Name = "Lösenord")]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }
    }
}