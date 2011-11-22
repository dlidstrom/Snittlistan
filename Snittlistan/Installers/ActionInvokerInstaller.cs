namespace Snittlistan.Installers
{
    using System.Web.Mvc;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Snittlistan.Infrastructure;

    public class ActionInvokerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            ////container.Register(
            ////    Component.For<IActionInvoker>()
            ////    .ImplementedBy<InjectingActionInvoker>()
            ////    .LifestyleTransient());
        }
    }
}