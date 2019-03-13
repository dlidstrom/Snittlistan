namespace Snittlistan.Web.Infrastructure.Attributes
{
    using System.Web.Http.Filters;

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