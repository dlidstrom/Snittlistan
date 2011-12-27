namespace Snittlistan.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Snittlistan.Controllers;

    /// <summary>
    /// Wraps another IActionInvoker except it handles the case of an action method
    /// not being found and invokes the NotFoundController instead.
    /// </summary>
    public class ActionInvokerWrapper : IActionInvoker
    {
        private readonly IActionInvoker actionInvoker;

        public ActionInvokerWrapper(IActionInvoker actionInvoker)
        {
            this.actionInvoker = actionInvoker;
        }

        public bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            if (actionInvoker.InvokeAction(controllerContext, actionName))
                return true;

            // No action method was found.
            var controller = new NotFoundController();
            controller.ExecuteNotFound(controllerContext.RequestContext);

            return true;
        }
    }
}