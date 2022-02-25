#nullable enable

using System.Data.Entity;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using EventStoreLite;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.Migration;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Commands;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Models;
using Snittlistan.Web.Services;

namespace Snittlistan.Web.Areas.V2.Controllers;

public class AdminTasksController : AdminController
{
    private readonly IAuthenticationService authenticationService;

    public AdminTasksController(IAuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService;
    }

    public ActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// GET: /User/Index.
    /// </summary>
    /// <returns></returns>
    public ActionResult Users()
    {
        List<UserViewModel> users = CompositionRoot.DocumentSession.Query<User>()
            .ToList()
            .Select(x => new UserViewModel(x))
            .OrderByDescending(x => x.IsActive)
            .ThenBy(x => x.Email)
            .ToList();

        return View(users);
    }

    public ActionResult CreateUser()
    {
        return View(new CreateUserViewModel());
    }

    [HttpPost]
    public ActionResult CreateUser(CreateUserViewModel vm)
    {
        // an existing user cannot be registered again
        if (CompositionRoot.DocumentSession.FindUserByEmail(vm.Email) != null)
        {
            ModelState.AddModelError("Email", "Användaren finns redan");
        }

        // redisplay form if any errors at this point
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        User newUser = new(string.Empty, string.Empty, vm.Email, string.Empty);
        CompositionRoot.DocumentSession.Store(newUser);

        return RedirectToAction("Users");
    }

    public ActionResult DeleteUser(string id)
    {
        User user = CompositionRoot.DocumentSession.Load<User>(id);
        if (user == null)
        {
            throw new HttpException(404, "User not found");
        }

        return View(new UserViewModel(user));
    }

    [HttpPost]
    [ActionName("DeleteUser")]
    public ActionResult DeleteUserConfirmed(string id)
    {
        User user = CompositionRoot.DocumentSession.Load<User>(id);
        if (user == null)
        {
            throw new HttpException(404, "User not found");
        }

        CompositionRoot.DocumentSession.Delete(user);
        return RedirectToAction("Users");
    }

    public ActionResult ActivateUser(string id)
    {
        User user = CompositionRoot.DocumentSession.Load<User>(id);
        if (user == null)
        {
            throw new HttpException(404, "User not found");
        }

        return View(new UserViewModel(user));
    }

    [HttpPost]
    [ActionName("ActivateUser")]
    public ActionResult ActivateUserConfirmed(string id, bool? invite)
    {
        User user = CompositionRoot.DocumentSession.Load<User>(id);
        if (user == null)
        {
            throw new HttpException(404, "User not found");
        }

        if (user.IsActive)
        {
            user.Deactivate();
        }
        else
        {
            if (invite.GetValueOrDefault())
            {
                Debug.Assert(Request.Url != null, "Request.Url != null");
                TaskPublisher taskPublisher = GetTaskPublisher();
                user.ActivateWithEmail(
                    t => taskPublisher.PublishTask(t, User.Identity.Name),
                    Url,
                    Request.Url!.Scheme);
            }
            else
            {
                user.Activate();
            }
        }

        return RedirectToAction("Users");
    }

    public ActionResult Raven()
    {
        return View();
    }

    public ActionResult ResetIndexes()
    {
        return View();
    }

    [HttpPost, ActionName("ResetIndexes")]
    public ActionResult ResetIndexesConfirmed()
    {
        IndexCreator.ResetIndexes(CompositionRoot.DocumentStore, CompositionRoot.EventStore);

        return RedirectToAction("Raven");
    }

    public ActionResult EventStoreActions()
    {
        return View();
    }

    public ActionResult ReplayEvents()
    {
        return View();
    }

    [HttpPost, ActionName("ReplayEvents")]
    public ActionResult ReplayEventsConfirmed()
    {
        EventStore.ReplayEvents(MvcApplication.ChildContainer);

        return RedirectToAction("EventStoreActions");
    }

    public ActionResult MigrateEvents()
    {
        return View("MigrateEvents", (HtmlString?)null);
    }

    [HttpPost, ActionName("MigrateEvents")]
    public ActionResult MigrateEventsConfirmed()
    {
        List<IEventMigratorWithResults> eventMigrators = MvcApplication.Container!.ResolveAll<IEventMigratorWithResults>()
            .ToList();
        CompositionRoot.EventStore.MigrateEvents(eventMigrators);
        string results = string.Join("", eventMigrators.Select(x => x.GetResults()));

        return View("MigrateEvents", new HtmlString(results));
    }

    public ActionResult SendMail()
    {
        return View(new SendMailViewModel());
    }

    [HttpPost]
    public async Task<ActionResult> SendMail(SendMailViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        int ratePerSeconds = vm.RateSetting! switch
        {
            SendMailViewModel.RateSettingValues.OnePerMinute => 60,
            SendMailViewModel.RateSettingValues.OnePerFiveMinutes => 5 * 60,
            SendMailViewModel.RateSettingValues.OnePerFifteenMinutes => 15 * 60,
            _ => 60
        };

        int mailCount = vm.MailCount! switch
        {
            SendMailViewModel.MailCountValues.One => 1,
            SendMailViewModel.MailCountValues.Five => 5,
            SendMailViewModel.MailCountValues.Fifteen => 15,
            _ => 1
        };

        for (int i = 0; i < mailCount; i++)
        {
            await ExecuteCommand(new SendEmailTaskCommandHandler.Command(
                vm.Recipient,
                vm.ReplyTo,
                vm.Subject,
                vm.Content,
                ratePerSeconds));
        }

        return RedirectToAction("Index");
    }

    public ActionResult Adopt()
    {
        Player[] players = CompositionRoot.DocumentSession.Query<Player, PlayerSearch>().ToArray();
        ViewBag.PlayerId = CompositionRoot.DocumentSession.CreatePlayerSelectList(
            getPlayers: () => players);
        return View();
    }

    [HttpPost]
    public ActionResult Adopt(string playerId)
    {
        authenticationService.SetAuthCookie(playerId, true);
        return RedirectToAction("Index", "Roster");
    }

    public async Task<ActionResult> Features()
    {
        KeyValueProperty? settingsProperty =
            await CompositionRoot.Databases.Snittlistan.KeyValueProperties.SingleOrDefaultAsync(
                x => x.Key == TenantFeatures.Key && x.TenantId == CompositionRoot.CurrentTenant.TenantId);
        if (settingsProperty != null)
        {
            return View(new TenantFeaturesViewModel((TenantFeatures)settingsProperty.Value));
        }

        return View(new TenantFeaturesViewModel());
    }

    [HttpPost]
    public async Task<ActionResult> Features(TenantFeaturesViewModel vm)
    {
        UpdateFeaturesCommandHandler.Command command = new(
            vm.RosterMailEnabled);
        await ExecuteCommand(command);

        return RedirectToAction("Index");
    }

    public class TenantFeaturesViewModel
    {
        public TenantFeaturesViewModel(TenantFeatures tenantFeatures)
        {
            RosterMailEnabled = tenantFeatures.RosterMailEnabled;
        }

        public TenantFeaturesViewModel()
        {
        }

        public bool RosterMailEnabled { get; set; }
    }
}
