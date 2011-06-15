using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;
using System.Web;

namespace SnittListan.IoC
{
	public class WindsorControllerFactory : DefaultControllerFactory
	{
		private readonly IKernel kernel;

		public WindsorControllerFactory(IKernel kernel)
		{
			this.kernel = kernel;
		}

		public override IController CreateController(RequestContext requestContext, string controllerName)
		{
			if (requestContext == null)
			{
				throw new ArgumentNullException("requestContext");
			}

			if (controllerName == null)
			{
				throw new HttpException(404, string.Format("The controller path '{0}' could not be found.", controllerName));
			}

			try
			{
				return kernel.Resolve<IController>(controllerName + "controller");
			}
			catch (ComponentNotFoundException ex)
			{
				throw new ApplicationException(string.Format("No controller with name '{0}' found", controllerName), ex);
			}
		}

		public override void ReleaseController(IController controller)
		{
			if (controller == null)
			{
				throw new ArgumentNullException("controller");
			}

			kernel.ReleaseComponent(controller);
		}
	}
}
