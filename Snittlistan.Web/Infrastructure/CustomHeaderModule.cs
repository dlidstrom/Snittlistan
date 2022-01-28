#nullable enable

using System.Web;

namespace Snittlistan.Web.Infrastructure;

public class CustomHeaderModule : IHttpModule
{
    public void Init(HttpApplication context)
    {
        context.PreSendRequestHeaders += OnPreSendRequestHeaders;
    }

    public void Dispose() { }

    private void OnPreSendRequestHeaders(object sender, EventArgs e)
    {
        // removes "Server" details from response header
        HttpContext.Current.Response.Headers.Remove("Server");
    }
}
