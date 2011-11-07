namespace Snittlistan.IoC
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Castle.MicroKernel;

    public class WindsorControllerFactory : DefaultControllerFactory
	{
		private readonly IKernel kernel;

		public WindsorControllerFactory(IKernel kernel)
		{
			this.kernel = kernel;
		}

        public override void ReleaseController(IController controller)
        {
            kernel.ReleaseComponent(controller);
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                return kernel.Resolve<IController>(controllerName);
            }
            catch (ComponentNotFoundException ex)
            {
                throw new ApplicationException(string.Format("No controller '{0}' found", controllerName), ex);
            }
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                return kernel.Resolve<IController>(controllerType.Name);
            }
            catch (ComponentNotFoundException ex)
            {
                throw new ApplicationException(string.Format("No controller '{0}' found", controllerType), ex);
            }
        }
    }
}