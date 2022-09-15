#nullable enable

using Snittlistan.Web.Commands;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Snittlistan.Web.Areas.V2.Controllers;

[Authorize]
public class UserProfileController : AbstractController
{
    public async Task<ActionResult> Index()
    {
        string key = UserSettings.GetKey(User!.CustomIdentity.PlayerId!);
        int tenantId = CompositionRoot.CurrentTenant.TenantId;
        KeyValueProperty? settingsProperty =
            await CompositionRoot.Databases.Snittlistan.KeyValueProperties.SingleOrDefaultAsync(
                x => x.Key == key && x.TenantId == tenantId);

        if (settingsProperty is not null)
        {
            return View(new UserSettingsViewModel((UserSettings)settingsProperty.Value));
        }

        return View(new UserSettingsViewModel(UserSettings.Default));
    }

    [HttpPost]
    public async Task<ActionResult> Save(UserSettingsViewModel vm)
    {
        string key = UserSettings.GetKey(User!.CustomIdentity.PlayerId!);
        UpdateUserSettingsCommandHandler.Command command = new(
            key,
            vm.RosterMailEnabled,
            vm.AbsenceMailEnabled,
            vm.MatchResultMailEnabled);
        await ExecuteCommand(command);
        FlashInfo("Inställningarna sparades.");
        return RedirectToAction("Index", "UserProfile");
    }

    public class UserSettingsViewModel
    {
        public UserSettingsViewModel(UserSettings userSettings)
        {
            RosterMailEnabled = userSettings.RosterMailEnabled;
            AbsenceMailEnabled = userSettings.AbsenceMailEnabled;
            MatchResultMailEnabled = userSettings.MatchResultMailEnabled;
        }

        public UserSettingsViewModel()
        {
        }

        [Display(
            Name = "Uttagning",
            GroupName = "Notifieringar")]
        public bool RosterMailEnabled { get; set; }

        [Display(
            Name = "Frånvaro",
            GroupName = "Notifieringar")]
        public bool AbsenceMailEnabled { get; set; }

        [Display(
            Name = "Match-resultat",
            GroupName = "Notifieringar")]
        public bool MatchResultMailEnabled { get; set; }
    }
}
