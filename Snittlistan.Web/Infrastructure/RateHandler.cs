namespace Snittlistan.Web.Infrastructure
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using NLog;

    public class RateHandler : DelegatingHandler
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly object locker = new object();
        private readonly double rate;
        private readonly double per;
        private readonly int maxTries;
        private readonly double sleepSeconds;
        private double allowance;
        private DateTime lastCheck;

        public RateHandler(double rate, double per, int maxTries, double sleepSeconds)
            : base(new HttpClientHandler())
        {
            if (rate < 1) throw new ArgumentOutOfRangeException(nameof(rate), "Rate must be >= 1");
            this.rate = rate;
            this.per = per;
            allowance = rate;
            this.maxTries = maxTries;
            this.sleepSeconds = sleepSeconds;
            lastCheck = DateTime.Now;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            var allowed = false;
            for (var currentTry = 0; currentTry < maxTries; currentTry++)
            {
                lock (locker)
                {
                    var current = DateTime.Now;
                    var timePassed = current - lastCheck;
                    lastCheck = current;
                    allowance += timePassed.TotalSeconds * (rate / per);

                    if (allowance > rate)
                    {
                        allowance = rate;
                        Logger.Info("Allowance exceeded rate ({0:F1} / {1:F1})", rate, per);
                    }
                    else
                    {
                        Logger.Info("Allowance = {0}", allowance);
                    }

                    if (allowance >= 1.0)
                    {
                        allowance--;
                        allowed = true;
                        Logger.Info("Allowance >= 1, request allowed");
                        break;
                    }

                    Logger.Info("Allowance < 1.0, delay");
                }

                // not yet, delay then try again
                await Task.Delay(TimeSpan.FromSeconds(sleepSeconds), cancellationToken);
            }

            if (!allowed) throw new Exception($"Failed after {maxTries} tries (elapsed = {sw.Elapsed.TotalSeconds:F1}s)");

            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}