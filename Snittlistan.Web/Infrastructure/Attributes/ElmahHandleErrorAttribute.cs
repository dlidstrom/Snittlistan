using System.Web;
using System.Web.Mvc;
using Elmah;
using NLog;

namespace Snittlistan.Web.Infrastructure.Attributes;
public class ElmahHandleErrorAttribute : HandleErrorAttribute
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);

        Exception e = context.Exception;
        if (!context.ExceptionHandled   // if unhandled, will be logged anyhow
            || RaiseErrorSignal(e)      // prefer signaling, if possible
            || IsFiltered(context))     // filtered?
        {
            return;
        }

        ErrorLog.GetDefault(HttpContext.Current).Log(new Error(e, HttpContext.Current));
        Log.Error(e);
    }

    private static bool RaiseErrorSignal(Exception e)
    {
        HttpContext context = HttpContext.Current;
        if (context == null)
        {
            return false;
        }

        var signal = ErrorSignal.FromContext(context);
        if (signal == null)
        {
            return false;
        }

        signal.Raise(e, context);
        return true;
    }

    private static bool IsFiltered(ExceptionContext context)
    {
        if (!(context.HttpContext.GetSection("elmah/errorFilter") is ErrorFilterConfiguration config))
        {
            return false;
        }

        var testContext = new ErrorFilterModule.AssertionHelperContext(
            context.Exception,
            HttpContext.Current);

        return config.Assertion.Test(testContext);
    }
}
