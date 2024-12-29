#nullable enable

using System.Net.Http;
using NLog;

namespace Snittlistan.Queue;

public class LoggingHandler : DelegatingHandler
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public LoggingHandler()
      : base(new HttpClientHandler())
    {
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Logger.Info(request.ToString());
        if (request.Content != null)
        {
            Logger.Info(await request.Content.ReadAsStringAsync());
        }

        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        Logger.Info(response.ToString());
        if (response.Content != null)
        {
            Logger.Info(await response.Content.ReadAsStringAsync());
        }

        return response;
    }
}
