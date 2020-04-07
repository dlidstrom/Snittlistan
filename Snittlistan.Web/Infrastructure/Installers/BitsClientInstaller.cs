namespace Snittlistan.Web.Infrastructure.Installers
{
    using System;
    using System.Net.Http;
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
            container.Register(Component.For<IBitsClient>().Instance(new BitsClient(apiKey, new HttpClient())));
        }
    }
}