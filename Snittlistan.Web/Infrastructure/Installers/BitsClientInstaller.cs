namespace Snittlistan.Web.Infrastructure.Installers
{
    using System;
    using System.Net.Http;
    using System.Runtime.Caching;
    using Bits;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using NLog;

    public class BitsClientInstaller : IWindsorInstaller
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var apiKey = Environment.GetEnvironmentVariable("ApiKey");
            Logger.Info("ApiKey: {0}", apiKey);
            var bitsClient = new BitsClient(
                apiKey,
                new HttpClient(new RateHandler(1.0, 1.0, 60, 0.5)),
                MemoryCache.Default);
            container.Register(Component.For<IBitsClient>().Instance(bitsClient));
        }
    }
}