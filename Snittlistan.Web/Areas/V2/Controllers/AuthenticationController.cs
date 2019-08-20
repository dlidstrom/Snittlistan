namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Domain;
    using Indexes;
    using Queue.Messages;
    using Raven.Abstractions;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Helpers;
    using Snittlistan.Web.Services;

    public class AuthenticationController : AbstractController
    {
        private static readonly Random Random = new Random();
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(EmailViewModel vm, string returnUrl)
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
                    players = DocumentSession.Query<Player, PlayerSearch>()
                                             .Where(x => x.Email == vm.Email
                                                         && (x.PlayerStatus == Player.Status.Active || x.PlayerStatus == Player.Status.Supporter))
                                             .ToArray();
                }

                if (players.Length == 0)
                {
                    ModelState.AddModelError("Email", "Spelare med den e-postadressen finns inte.");
                }
                else if (players.Length == 1)
                {
                    var player = players[0];

                    // if player already has non-expired token, then reuse that one,
                    // else create a new token
                    var existingTokens =
                        DocumentSession.Query<OneTimeToken, OneTimeTokenIndex>()
                                       .Where(x => x.PlayerId == player.Id && x.CreatedDate > SystemTime.UtcNow.AddDays(-1))
                                       .Take(10)
                                       .ToArray();
                    var validExistingToken = existingTokens.FirstOrDefault(x => x.IsExpired() == false && x.UsedDate.HasValue == false);
                    if (validExistingToken != null)
                    {
                        // reuse still valid token
                        return RedirectToAction("LogOnOneTimePassword", new
                        {
                            id = player.Id,
                            validExistingToken.OneTimeKey
                        });
                    }
                    else
                    {
                        // no valid token, generate new
                        var token = new OneTimeToken(player.Id);
                        Debug.Assert(Request.Url != null, "Request.Url != null");
                        var oneTimePassword =
                            string.Join("", Enumerable.Range(1, 6).Select(_ => Random.Next(10)));
                        token.Activate(
                            oneTimeKey =>
                            {
                                PublishMessage(new OneTimeKeyEvent(player.Email, oneTimePassword));
                            },
                            oneTimePassword);
                        DocumentSession.Store(token);
                        return RedirectToAction("LogOnOneTimePassword", new { id = player.Id, token.OneTimeKey });
                    }
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

        public ActionResult LogOnOneTimePassword(string id, string oneTimeKey)
        {
            var player = DocumentSession.Load<Player>(id);
            return View(new PasswordViewModel
            {
                Email = player.Email,
                RememberMe = true,
                OneTimeKey = oneTimeKey
            });
        }

        [HttpPost]
        public ActionResult LogOnOneTimePassword(string id, PasswordViewModel vm)
        {
            if (Request.IsAuthenticated) return RedirectToAction("Index", "Roster");

            var oneTimeToken = DocumentSession.Query<OneTimeToken, OneTimeTokenIndex>().Single(x => x.OneTimeKey == vm.OneTimeKey);
            var player = DocumentSession.Load<Player>(id);
            if (player == null)
                throw new HttpException(404, "Player not found");
            switch (oneTimeToken.ApplyToken())
            {
                case OneTimeToken.Result.Ok:
                {
                    try
                    {
                        if (vm.Password != oneTimeToken.Payload)
                        {
                            ModelState.AddModelError("Lösenord", "Felaktigt lösenord");
                            vm.Password = string.Empty;
                            return View();
                        }

                        authenticationService.SetAuthCookie(player.Id, vm.RememberMe);
                        var builder = new StringBuilder();
                        builder.AppendLine($"User Agent: {Request.UserAgent}");
                        PublishMessage(
                            EmailTask.Create(
                                ConfigurationManager.AppSettings["OwnerEmail"],
                                $"{player.Name} logged in",
                                builder.ToString()));
                    }
                    catch
                    {
                        //
                    }

                    return RedirectToAction("Index", "Roster");
                }
                case OneTimeToken.Result.Expired:
                {
                    return View("TokenExpired");
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

            public string OneTimeKey { get; set; }
        }
    }
}