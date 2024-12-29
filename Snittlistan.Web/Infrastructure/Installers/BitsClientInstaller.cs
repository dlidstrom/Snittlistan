#nullable enable

using System.Net.Http;
using System.Runtime.Caching;
using Snittlistan.Web.Infrastructure.Bits;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Snittlistan.Web.Infrastructure.Installers;

public class BitsClientInstaller(HttpClient httpClient) : IWindsorInstaller
{
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
        BitsClient bitsClient = new(
            httpClient,
            MemoryCache.Default);
        _ = container.Register(Component.For<IBitsClient>().Instance(bitsClient));
    }
}
