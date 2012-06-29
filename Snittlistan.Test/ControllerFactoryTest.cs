namespace Snittlistan.Test
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Castle.Windsor;
    using Controllers;
    using Infrastructure.Installers;
    using Infrastructure.IoC;
    using MvcContrib.TestHelper;
    using Xunit;

    public class ControllerFactoryTest
    {
        [Fact]
        public void CanCreateController()
        {
            var container = new WindsorContainer()
                .Install(new ControllerFactoryInstaller())
                .Install(new ControllerInstaller());

            IControllerFactory factory = null;
            Assert.DoesNotThrow(() => factory = container.Resolve<IControllerFactory>());
            IController controller = null;
            Assert.DoesNotThrow(() => controller = factory.CreateController(new RequestContext(), typeof(ErrorController).Name.Replace("Controller", string.Empty)));
            controller.ShouldNotBeNull("Failed to create controller");
        }
    }
}