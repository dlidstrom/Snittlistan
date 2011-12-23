namespace Snittlistan.IoC
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Castle.MicroKernel;
    using Snittlistan.Controllers;

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
            if (requestContext == null)
                throw new ArgumentNullException("requestContext");
            if (controllerName == null)
                throw new ArgumentNullException("controllerName");

            try
            {
                return kernel.Resolve<IController>(controllerName + "Controller");
            }
            catch (ComponentNotFoundException)
            {
                return new NotFoundController();
            }
        }
    }
}