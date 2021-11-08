namespace Snittlistan.Web.Infrastructure.IoC
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Castle.MicroKernel;
    using Snittlistan.Web.Areas.V1.Controllers;

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
            {
                throw new ArgumentNullException(nameof(requestContext));
            }

            if (controllerName == null)
            {
                throw new ArgumentNullException(nameof(controllerName));
            }

            try
            {
                IController controller = kernel.Resolve<IController>(controllerName + "Controller");
                if (controller is Controller controllerWithInvoker)
                {
                    controllerWithInvoker.ActionInvoker = new ActionInvokerWrapper(controllerWithInvoker.ActionInvoker);
                }

                return controller;
            }
            catch (ComponentNotFoundException)
            {
                return new NotFoundController();
            }
        }
    }
}