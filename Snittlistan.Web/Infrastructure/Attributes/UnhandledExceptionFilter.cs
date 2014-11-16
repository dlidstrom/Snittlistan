using System.Web.Http.Filters;

namespace Snittlistan.Web.Infrastructure.Attributes
{
    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception);
            }

            base.OnException(actionExecutedContext);
        }
    }
}