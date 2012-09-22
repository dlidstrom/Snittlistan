namespace Snittlistan.Web.Controllers
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using Snittlistan.Web.Infrastructure.Results;

    public class NotFoundController : IController
    {
        public void Execute(RequestContext requestContext)
        {
            this.ExecuteNotFound(requestContext);
        }

        public void ExecuteNotFound(RequestContext requestContext)
        {
            new NotFoundViewResult().ExecuteResult(new ControllerContext(requestContext, new FakeController()));
        }

        // ControllerContext requires an object that derives from ControllerBase.
        // NotFoundController does not do this.
        // So the easiest workaround is this FakeController.
        public class FakeController : Controller
        {
        }
    }
}
