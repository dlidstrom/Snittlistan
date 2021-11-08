#nullable enable

namespace Snittlistan.Test
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Castle.Windsor;
    using NUnit.Framework;
    using Snittlistan.Web.Infrastructure.Installers;
    using Snittlistan.Web.Infrastructure.IoC;
    using Web.Areas.V2.Controllers;

    [TestFixture]
    public class ControllerFactoryTest
    {
        [Test]
        public void CanCreateController()
        {
            IWindsorContainer container = new WindsorContainer()
                .Install(new ControllerFactoryInstaller())
                .Install(new ControllerInstaller());

            IControllerFactory? factory = null;
            Assert.DoesNotThrow(() => factory = container.Resolve<IControllerFactory>());
            IController? controller = null;
            Assert.DoesNotThrow(() => controller = factory!.CreateController(new RequestContext(), nameof(ErrorController).Replace("Controller", string.Empty)));
            Assert.NotNull(controller);
        }
    }
}
