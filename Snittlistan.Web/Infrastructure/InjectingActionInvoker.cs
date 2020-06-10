namespace Snittlistan.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;
    using Castle.MicroKernel;
    using Castle.MicroKernel.ComponentActivator;

    public class InjectingActionInvoker : ControllerActionInvoker
    {
        private readonly IKernel container;

        public InjectingActionInvoker(IKernel container)
        {
            this.container = container;
        }

        public static void InjectProperties(IKernel kernel, object target)
        {
            Type type = target.GetType();

            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanWrite && kernel.HasComponent(property.PropertyType))
                {
                    object value = kernel.Resolve(property.PropertyType);
                    try
                    {
                        property.SetValue(target, value, null);
                    }
                    catch (Exception ex)
                    {
                        string message = $"Error setting property {property.Name} on type {type.FullName}, See inner exception for more information.";
                        throw new ComponentActivatorException(message, ex, null);
                    }
                }
            }
        }

        protected override ActionExecutedContext InvokeActionMethodWithFilters(ControllerContext controllerContext, IList<IActionFilter> filters, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
        {
            foreach (IActionFilter filter in filters)
            {
                InjectProperties(container, filter);
            }

            return base.InvokeActionMethodWithFilters(controllerContext, filters, actionDescriptor, parameters);
        }
    }
}