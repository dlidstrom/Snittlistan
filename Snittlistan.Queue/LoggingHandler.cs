namespace Snittlistan.Queue
{
    using System.Net.Http;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using log4net;

    public class LoggingHandler : DelegatingHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Log.Info(request.ToString());
            if (request.Content != null)
            {
                Log.Info(await request.Content.ReadAsStringAsync());
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            Log.Info(response.ToString());
            if (response.Content != null)
            {
                Log.Info(await response.Content.ReadAsStringAsync());
            }

            return response;
        }
    }
}