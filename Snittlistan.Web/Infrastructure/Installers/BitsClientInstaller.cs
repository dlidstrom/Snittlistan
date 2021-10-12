#nullable enable

namespace Snittlistan.Web.Infrastructure.Installers
{
    using System.Net.Http;
    using System.Runtime.Caching;
    using Bits;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class BitsClientInstaller : IWindsorInstaller
    {
        private readonly string apiKey;
        private readonly HttpClient httpClient;

        public BitsClientInstaller(string apiKey, HttpClient httpClient)
        {
            this.apiKey = apiKey;
            this.httpClient = httpClient;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            BitsClient bitsClient = new(
                apiKey,
                httpClient,
                MemoryCache.Default);
            _ = container.Register(Component.For<IBitsClient>().Instance(bitsClient));
        }
    }
}
