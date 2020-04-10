namespace Snittlistan.Web.Infrastructure.Installers
{
    using System;
    using System.Net.Http;
    using Bits;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class BitsClientInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var apiKey = Environment.GetEnvironmentVariable("ApiKey");
            container.Register(Component.For<IBitsClient>().Instance(new BitsClient(apiKey, new HttpClient())));
        }
    }
}