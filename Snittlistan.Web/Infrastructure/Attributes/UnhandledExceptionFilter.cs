namespace Snittlistan.Web.Infrastructure.Attributes
{
    using System.Web.Http.Filters;
    using NLog;

    public class UnhandledExceptionFilter : ExceptionFilterAttribute
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(actionExecutedContext.Exception);
                Logger.Error(actionExecutedContext.Exception);
            }

            base.OnException(actionExecutedContext);
        }
    }
}