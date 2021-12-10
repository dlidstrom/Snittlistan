using System.Web;
using System.Web.Mvc;
using Snittlistan.Web.Areas.V2.ViewModels;
using Snittlistan.Web.Controllers;
using Snittlistan.Web.Models;
using Snittlistan.Web.Services;

namespace Snittlistan.Web.Areas.V2.Controllers;
/// <summary>
/// User administration.
/// </summary>
public class UserController : AbstractController
{
    private readonly IAuthenticationService authenticationService;

    /// <summary>
    /// Initializes a new instance of the UserController class.
    /// </summary>
    /// <param name="authenticationService">Authentication service.</param>
    public UserController(IAuthenticationService authenticationService)
    {
        this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }

    public ActionResult SetPassword(string id, string activationKey)
    {
        User user = DocumentSession.Load<User>(id);
        if (user == null)
        {
            throw new HttpException(404, "User not found");
        }

        if (user.ActivationKey != activationKey)
        {
            throw new InvalidOperationException("Unknown activation key");
        }

        return View(new SetPasswordViewModel { ActivationKey = activationKey });
    }

    [HttpPost]
    public ActionResult SetPassword(string id, SetPasswordViewModel vm)
    {
        User user = DocumentSession.Load<User>(id);
        if (user == null)
        {
            throw new HttpException(404, "User not found");
        }

        if (ModelState.IsValid == false)
        {
            return View(vm);
        }

        user.SetPassword(vm.Password, vm.ActivationKey);
        authenticationService.SetAuthCookie(user.Email, true);
        return RedirectToAction("Index", "Roster");
    }
}
