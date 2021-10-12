#nullable enable

namespace Snittlistan.Web.Infrastructure.Installers
{
    using System.Web.Http;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class ApiControllerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            _ = container.Register(
                Classes.FromThisAssembly().BasedOn<ApiController>().LifestyleTransient());
        }
    }
}
