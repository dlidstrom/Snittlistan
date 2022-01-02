
using System.Web.Mvc;
using Elmah;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Areas.V2.Controllers;
public class ErrorController : Controller
{
    public void LogJavaScriptError(string message)
    {
        ErrorSignal
            .FromCurrentContext()
            .Raise(new JavaScriptException(message));
    }
}
