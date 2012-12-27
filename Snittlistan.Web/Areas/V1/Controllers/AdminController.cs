namespace Snittlistan.Web.Areas.V1.Controllers
{
    using System.Security.Authentication;
    using System.Web.Mvc;

    using Raven.Client;

    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Models;

    [Authorize]
    public abstract class AdminController : AbstractController
    {
        protected AdminController(IDocumentSession session)
            : base(session)
        { }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Session.Load<User>("Admin").Email != filterContext.HttpContext.User.Identity.Name)
                throw new AuthenticationException("Only Admin allowed");
        }
    }
}