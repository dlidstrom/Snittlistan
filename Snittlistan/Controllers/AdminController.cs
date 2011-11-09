namespace Snittlistan.Controllers
{
    using System.Web.Mvc;
    using Raven.Client;

    [Authorize]
    public abstract class AdminController : AbstractController
    {
        public AdminController(IDocumentSession session)
            : base(session)
        { }
    }
}