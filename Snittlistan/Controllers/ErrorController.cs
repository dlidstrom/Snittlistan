namespace Snittlistan.Controllers
{
    using System.Web.Mvc;
    using Elmah;
    using Snittlistan.Helpers;

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