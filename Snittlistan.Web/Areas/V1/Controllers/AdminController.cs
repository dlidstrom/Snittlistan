namespace Snittlistan.Web.Areas.V1.Controllers
{
    using System.Web.Mvc;

    using Raven.Client;

    using Snittlistan.Web.Controllers;

    [Authorize]
    public abstract class AdminController : AbstractController
    {
        protected AdminController(IDocumentSession session)
            : base(session)
        { }
    }
}