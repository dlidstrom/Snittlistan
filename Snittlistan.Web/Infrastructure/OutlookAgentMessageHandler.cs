using System.Net.Http;

namespace Snittlistan.Web.Infrastructure;
public class OutlookAgentMessageHandler
    : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Headers.UserAgent.Any(x => x.Comment != null && x.Comment.Contains("Microsoft Outlook")))
        {
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/iCal"));
        }
        return base.SendAsync(request, cancellationToken);
    }
}
