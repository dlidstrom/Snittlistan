namespace Snittlistan.Web.Infrastructure.Installers
{
    using global::AutoMapper;

    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class AutoMapperInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(FindProfiles().LifestyleSingleton().WithServiceFromInterface(typeof(Profile)));
        }

        private static BasedOnDescriptor FindProfiles()
        {
            return AllTypes
                .FromThisAssembly()
                .BasedOn<Profile>();
        }
    }
}