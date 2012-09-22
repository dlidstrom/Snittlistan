namespace Snittlistan.Web.Infrastructure.IoC
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Castle.MicroKernel;

    using Snittlistan.Web.Controllers;

    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel kernel;

        public WindsorControllerFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public override void ReleaseController(IController controller)
        {
            this.kernel.ReleaseComponent(controller);
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            if (requestContext == null)
                throw new ArgumentNullException("requestContext");
            if (controllerName == null)
                throw new ArgumentNullException("controllerName");

            try
            {
                var controller = this.kernel.Resolve<IController>(controllerName + "Controller");
                var controllerWithInvoker = controller as Controller;
                if (controllerWithInvoker != null)
                    controllerWithInvoker.ActionInvoker = new ActionInvokerWrapper(controllerWithInvoker.ActionInvoker);
                return controller;
            }
            catch (ComponentNotFoundException)
            {
                return new NotFoundController();
            }
        }
    }
}