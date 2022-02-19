#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Queue.Messages;
using Raven.Abstractions;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.HtmlHelpers;
using Snittlistan.Web.Infrastructure.Attributes;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Services;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Areas.V2.Controllers;

public class AuthenticationController : AbstractController
{
    private static readonly Random Random = new();
    private readonly IAuthenticationService authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }

    [RestoreModelStateFromTempData]
    public ActionResult LogOn()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> LogOn(EmailViewModel vm, string returnUrl)
    {
        // find the user in question
        Models.User user = CompositionRoot.DocumentSession.FindUserByEmail(vm.Email);

        if (user == null)
        {
            // locate player
            Player[] players;
            if (vm.PlayerId != null)
            {
                players = new[]
                {
                    CompositionRoot.DocumentSession.Load<Player>(vm.PlayerId)
                };
            }
            else
            {
                Player[] possiblePlayers = CompositionRoot.DocumentSession
                    .Query<Player, PlayerSearch>()
                    .Where(x => x.PlayerStatus == Player.Status.Active
                        || x.PlayerStatus == Player.Status.Supporter)
                    .ToArray();
                players = possiblePlayers
                    .Where(x => x.Email is not null)
                    .Select(x => new PossiblePlayer(x, x.Email.EditDistanceTo(vm.Email)))
                    .Where(x => x.EditDistance <= 3)
                    .OrderBy(x => x.EditDistance)
                    .Take(1)
                    .Select(x => x.Player)
                    .ToArray()
                    ?? Array.Empty<Player>();
            }

            if (players.Length == 0)
            {
                ModelState.AddModelError("Email", "Spelare med den e-postadressen finns inte.");
            }
            else if (players.Length == 1)
            {
                Player player = players[0];

                // if player already has non-expired token, then reuse that one,
                // else create a new token
                OneTimeToken[] existingTokens =
                    CompositionRoot.DocumentSession.Query<OneTimeToken, OneTimeTokenIndex>()
                                   .Where(x => x.PlayerId == player.Id)
                                   .OrderByDescending(x => x.CreatedDate)
                                   .Take(10)
                                   .ToArray();
                OneTimeToken validExistingToken = existingTokens.FirstOrDefault(x => x.IsExpired() == false);
                if (validExistingToken != null)
                {
                    // reuse still valid token
                    await NotifyEvent($"{player.Name} - Samma token", validExistingToken.ToJson().ToString());
                    return RedirectToAction(
                        "LogOnOneTimePassword",
                        new { id = player.Id, validExistingToken.OneTimeKey, reuseToken = true });
                }

                // no valid token, generate new
                OneTimeToken token = new(player.Id);
                Debug.Assert(Request.Url != null, "Request.Url != null");
                string oneTimePassword =
                    string.Join("", Enumerable.Range(1, 6).Select(_ => Random.Next(10)));
                Tenant tenant = await CompositionRoot.GetCurrentTenant();
                TaskPublisher taskPublisher = await GetTaskPublisher();
                token.Activate(
                    oneTimeKey =>
                        taskPublisher.PublishTask(
                            new OneTimeKeyTask(player.Email, oneTimePassword),
                            "system")
                    ,
                    oneTimePassword);
                await NotifyEvent($"{player.Name} entered email address");
                CompositionRoot.DocumentSession.Store(token);
                return RedirectToAction(
                    "LogOnOneTimePassword",
                    new { id = player.Id, token.OneTimeKey });
            }
            else if (players.Length > 1)
            {
                ViewBag.PlayerId = CompositionRoot.DocumentSession.CreatePlayerSelectList(
                    getPlayers: () => players,
                    textFormatter: p => $"{p.Name} ({p.Nickname})");
                await NotifyEvent($"{vm.Email} - Select from multiple {string.Join(", ", players.Select(x => $"{x.Name} ({x.Email})"))}");
                return View();
            }
            else
            {
                throw new Exception("Unhandled case");
            }
        }

        // redisplay form if any errors at this point
        if (ModelState.IsValid == false)
        {
            await NotifyEvent($"{vm.Email} - ModelState invalid: {string.Join(", ", ModelState.Values.Select(x => string.Join(", ", x.Errors.Select(y => y.ErrorMessage))))}");
            return View(vm);
        }

        return RedirectToAction("LogOnPassword", new { vm.Email, returnUrl });
    }

    [RestoreModelStateFromTempData]
    public async Task<ActionResult> LogOnOneTimePassword(string id, string oneTimeKey, bool? reuseToken)
    {
        DateTimeOffset? tokenDate = null;
        if (reuseToken ?? false)
        {
            OneTimeToken reusedToken = CompositionRoot.DocumentSession.Query<OneTimeToken, OneTimeTokenIndex>()
                .SingleOrDefault(x => x.OneTimeKey == oneTimeKey);
            tokenDate = reusedToken?.Timestamp;
        }

        Tenant tenant = await CompositionRoot.GetCurrentTenant();
        Player player = CompositionRoot.DocumentSession.Load<Player>(id);
        return View(new PasswordViewModel
        {
            Email = player.Email,
            RememberMe = true,
            OneTimeKey = oneTimeKey,
            ReuseToken = reuseToken ?? false,
            ReusedTokenDate = tokenDate,
            Hostname = tenant.Hostname
        });
    }

    [HttpPost]
    [SetTempModelState]
    public async Task<ActionResult> LogOnOneTimePassword(string id, PasswordViewModel vm)
    {
        if (Request.IsAuthenticated)
        {
            return RedirectToAction("Index", "Roster");
        }

        OneTimeToken[] activeTokens = CompositionRoot.DocumentSession.Query<OneTimeToken, OneTimeTokenIndex>()
            .Where(x => x.PlayerId == id && x.CreatedDate > SystemTime.UtcNow.ToLocalTime().AddDays(-1))
            .ToArray();
        Player player = CompositionRoot.DocumentSession.Load<Player>(id);
        if (player == null)
        {
            throw new HttpException(404, "Player not found");
        }

        try
        {
            if (activeTokens.Any() == false)
            {
                Logger.Info("No tokens");
                ModelState.AddModelError("Lösenord", "Prova igen");
                vm.Password = string.Empty;
                await Task.Delay(2000);
                await NotifyEvent($"{player.Name} - Prova igen");
                return View(vm);
            }

            OneTimeToken matchingPassword =
                activeTokens.FirstOrDefault(x => x.Payload!.EditDistanceTo(vm.Password) <= 1);
            if (matchingPassword == null)
            {
                Logger.Info("No matching password token");
                ModelState.AddModelError("Lösenord", "Felaktigt lösenord");
                vm.Password = string.Empty;
                await Task.Delay(2000);
                await NotifyEvent($"{player.Name} - Felaktig kod ({vm.Password})");
                return View(vm);
            }

            authenticationService.SetAuthCookie(player.Id, vm.RememberMe);
            await NotifyEvent($"{player.Name} logged in");
        }
        catch
        {
            //
        }

        return RedirectToAction("Index", "Roster");
    }

    public ActionResult LogOnPassword(string email, string returnUrl)
    {
        return View(new PasswordViewModel { Email = email, RememberMe = true, ReturnUrl = returnUrl });
    }

    [HttpPost]
    public ActionResult LogOnPassword(string returnUrl, PasswordViewModel vm)
    {
        // find the user in question
        Models.User user = CompositionRoot.DocumentSession.FindUserByEmail(vm.Email);

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
        {
            return View(vm);
        }

        authenticationService.SetAuthCookie(user.Email, vm.RememberMe);

        return Url.IsLocalUrl(returnUrl)
            && returnUrl.Length > 1
            && returnUrl.StartsWith("/")
            && !returnUrl.StartsWith("//")
            && !returnUrl.StartsWith("/\\")
            ? Redirect(returnUrl)
            : RedirectToAction("Index", "Roster");
    }

    public async Task<ActionResult> LogOff()
    {
        if (Request.IsAuthenticated)
        {
            await NotifyEvent($"{User.CustomIdentity.Name} logged off");
            authenticationService.SignOut();
        }

        return RedirectToAction("Index", "Roster");
    }

    private async Task NotifyEvent(string subject, string? body = null)
    {
        SendEmailTask task = SendEmailTask.Create(
            ConfigurationManager.AppSettings["OwnerEmail"],
            string.Empty,
            subject,
            string.Join(
                Environment.NewLine,
                new[]
                {
                        $"User Agent: {Request.UserAgent}",
                        $"Referrer: {Request.UrlReferrer}",
                        body ?? string.Empty
                }),
            60);
        TaskPublisher taskPublisher = await GetTaskPublisher();
        taskPublisher.PublishTask(
            task,
            "system");
    }

    public class EmailViewModel
    {
        [Required(ErrorMessage = "Ange e-postadress")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-postadress")]
        public string Email { get; set; } = null!;

        public string? PlayerId { get; set; }
    }

    public class PasswordViewModel
    {
        [Required(ErrorMessage = "Ange e-postadress")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-postadress")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Ange lösenord")]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }

        public string OneTimeKey { get; set; } = null!;

        public bool ReuseToken { get; set; }

        public DateTimeOffset? ReusedTokenDate { get; set; }

        public string? ReturnUrl { get; set; }

        public string? Hostname { get; set; }
    }

    public record PossiblePlayer(Player Player, int EditDistance);
}
