using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NLog;

#nullable enable

namespace Snittlistan.Web.Infrastructure.Attributes;
public class UserTrackerLogAttribute : ActionFilterAttribute
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        if (filterContext.RequestContext.HttpContext.Request.IsAuthenticated == false)
        {
            return;
        }

        ActionDescriptor actionDescriptor = filterContext.ActionDescriptor;
        string controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
        string actionName = actionDescriptor.ActionName;
        string userName = filterContext.HttpContext.User.Identity.Name;
        string routeId = string.Empty;
        if (filterContext.RouteData.Values["id"] != null)
        {
            routeId = filterContext.RouteData.Values["id"].ToString();
        }

        StringBuilder message = new();
        _ = message
            .AppendFormat("UserName={0}|", userName)
            .AppendFormat("RemoteIp={0}|", GetIp(filterContext.HttpContext.Request))
            .AppendFormat("Controller={0}|", controllerName)
            .AppendFormat("Action={0}|", actionName);
        if (!string.IsNullOrEmpty(routeId))
        {
            _ = message.AppendFormat("RouteId={0}|", routeId);
        }

        Log.Info(message.ToString());
        base.OnActionExecuted(filterContext);
    }

    /// <summary>
    /// Gets the IP address of the request.
    /// This method is more useful than built in because in
    /// some cases it may show real user IP address even under proxy.
    /// The <see cref="IPAddress.None" /> value
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

        IPAddress result = IPAddress.None;
        if (remoteAddress != null)
        {
            _ = IPAddress.TryParse(remoteAddress, out result);
        }

        return result;
    }
}
