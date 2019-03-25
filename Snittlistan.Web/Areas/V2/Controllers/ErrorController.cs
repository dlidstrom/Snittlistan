namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System.Web.Mvc;
    using Elmah;
    using Infrastructure;

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