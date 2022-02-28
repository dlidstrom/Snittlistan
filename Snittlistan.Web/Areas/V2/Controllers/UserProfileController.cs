#nullable enable

using Snittlistan.Web.Controllers;
using Snittlistan.Web.Infrastructure.Database;
using System.Data.Entity;
using System.Web.Mvc;

namespace Snittlistan.Web.Areas.V2.Controllers;

[Authorize]
public class UserProfileController : AbstractController
{
    public async Task<ActionResult> Index()
    {
        string key = UserSettings.GetKey(User.CustomIdentity.PlayerId!);
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

    public class UserSettingsViewModel
    {
        public UserSettingsViewModel(UserSettings userSettings)
        {
            RosterMailEnabled = userSettings.RosterMailEnabled;
        }

        public UserSettingsViewModel()
        {
        }

        public bool RosterMailEnabled { get; set; }
    }
}
