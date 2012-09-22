namespace Snittlistan.Web.Infrastructure.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Castle.Windsor;

    public class WindsorDependencyResolver : IDependencyResolver
    {
        private IWindsorContainer container;

        public WindsorDependencyResolver(IWindsorContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            object service = null;
            if (this.container.Kernel.HasComponent(serviceType))
                service = this.container.Resolve(serviceType);

            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            IEnumerable<object> services = new object[] { };
            if (this.container.Kernel.HasComponent(serviceType))
                services = this.container.ResolveAll(serviceType).Cast<object>();

            return services;
        }
    }
}