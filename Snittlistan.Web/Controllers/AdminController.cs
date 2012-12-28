namespace Snittlistan.Web.Controllers
{
    using System.Security.Authentication;
    using System.Web.Mvc;

    using Raven.Client;

    using Snittlistan.Web.Models;

    [Authorize]
    public abstract class AdminController : AbstractController
    {
        protected AdminController(IDocumentSession session)
            : base(session)
        { }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (this.Session.Load<User>("Admin").Email != filterContext.HttpContext.User.Identity.Name)
                throw new AuthenticationException("Only Admin allowed");
        }
    }
}