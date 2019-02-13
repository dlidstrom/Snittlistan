namespace Snittlistan.Web.Controllers
{
    using System.Web.Mvc;
    using Snittlistan.Web.Models;

    [Authorize]
    public abstract class AdminController : AbstractController
    {
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (DocumentSession.Load<User>("Admin").Email != filterContext.HttpContext.User.Identity.Name)
                filterContext.Result = new HttpUnauthorizedResult("Only Admin allowed");
        }
    }
}