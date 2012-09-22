namespace Snittlistan.Web.Controllers
{
    using System.Web.Mvc;

    using Raven.Client;
    using Raven.Imports.Newtonsoft.Json;

    public abstract class ApiController : AbstractController
    {
        protected ApiController(IDocumentSession session) : base(session)
        {
        }

        public new ActionResult Json(object result)
        {
            return new JsonResult(result);
        }

        private class JsonResult : ActionResult
        {
            private readonly object result;

            public JsonResult(object result)
            {
                this.result = result;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.ContentType = "application/json";

                var serializer = new JsonSerializer { Formatting = Formatting.Indented };
                serializer.Serialize(context.HttpContext.Response.Output, this.result);
            }
        }
    }
}