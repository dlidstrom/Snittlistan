using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using SnittListan.Controllers;
using SnittListan.Installers;
using SnittListan.IoC;
using Xunit;

namespace SnittListan.Test
{
	public class ControllerFactoryTest
	{
		[Fact]
		public void CanCreateHomeController()
		{
			var container = new WindsorContainer()
				.Install(new ControllerFactoryInstaller())
				.Install(new ControllerInstaller());

			IControllerFactory factory = null;
			Assert.DoesNotThrow(() => factory = container.Resolve<IControllerFactory>());
			IController controller = null;
			Assert.DoesNotThrow(() => controller = factory.CreateController(new RequestContext(), typeof(HomeController).Name.Replace("Controller", string.Empty)));
			Assert.NotNull(controller);
		}
	}
}
