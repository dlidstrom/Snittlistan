namespace Snittlistan.Web.Controllers
{
    using System.Web.Mvc;

    using Raven.Client;

    [Authorize]
    public abstract class AdminController : AbstractController
    {
        protected AdminController(IDocumentSession session)
            : base(session)
        { }
    }
}