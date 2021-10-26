#nullable enable

namespace Snittlistan.Queue
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using NLog;

    public class LoggingHandler : DelegatingHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
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
}
