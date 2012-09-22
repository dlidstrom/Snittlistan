namespace Snittlistan.Web.Infrastructure.Attributes
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    using NLog;

    public class UserTrackerLogAttribute : ActionFilterAttribute
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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
            message.AppendFormat("RemoteIp={0}|", GetIp(filterContext.HttpContext.Request));
            message.AppendFormat("Controller={0}|", controllerName);
            message.AppendFormat("Action={0}|", actionName);
            message.AppendFormat("TimeStamp={0}|", timeStamp);
            if (!string.IsNullOrEmpty(routeId))
                message.AppendFormat("RouteId={0}|", routeId);

            Log.Info(message.ToString());
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// Gets the IP address of the request.
        /// This method is more useful than built in because in
        /// some cases it may show real user IP address even under proxy.
        /// The <see cref="System.Net.IPAddress.None" /> value
        /// will be returned if getting is failed.
        /// </summary>
        /// <param name="request">The HTTP request object.</param>
        /// <returns>IPAddress object.</returns>
        private static IPAddress GetIp(HttpRequestBase request)
        {
            string remoteAddress;
            if (string.IsNullOrEmpty(request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
            {
                remoteAddress = request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                remoteAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                   .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                   .FirstOrDefault();
            }

            IPAddress result;
            if (!IPAddress.TryParse(remoteAddress, out result))
                result = IPAddress.None;

            return result;
        }
    }
}