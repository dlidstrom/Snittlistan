namespace Snittlistan.Web.Areas.V1.Controllers
{
    using System.Web.Mvc;

    using Elmah;

    using Snittlistan.Web.Infrastructure;

    public class ErrorController : Controller
    {
        public void LogJavaScriptError(string message)
        {
            ErrorSignal
                .FromCurrentContext()
                .Raise(new JavaScriptException(message));
        }
    }
}