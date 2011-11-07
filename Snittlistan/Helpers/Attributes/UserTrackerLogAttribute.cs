namespace Snittlistan.Helpers.Attributes
{
    using System;
    using System.Text;
    using System.Web.Mvc;
    using Common.Logging;

    public class UserTrackerLogAttribute : ActionFilterAttribute
    {
        private static readonly ILog logger = LogManager.GetCurrentClassLogger();

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var actionDescriptor = filterContext.ActionDescriptor;
            string controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = actionDescriptor.ActionName;
            string userName = filterContext.HttpContext.User.Identity.Name.ToString();
            DateTime timeStamp = filterContext.HttpContext.Timestamp;
            string routeId = string.Empty;
            if (filterContext.RouteData.Values["id"] != null)
                routeId = filterContext.RouteData.Values["id"].ToString();

            var message = new StringBuilder();
            message.AppendFormat("UserName={0}|", userName);
            message.AppendFormat("Controller={0}|", controllerName);
            message.AppendFormat("Action={0}|", actionName);
            message.AppendFormat("TimeStamp={0}|", timeStamp);
            if (!string.IsNullOrEmpty(routeId))
                message.AppendFormat("RouteId={0}|", routeId);

            logger.Info(message.ToString());
            base.OnActionExecuted(filterContext);
        }
    }
}