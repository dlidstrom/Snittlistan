using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.MicroKernel;

namespace Snittlistan.Web.Infrastructure.IoC
{
    public sealed class WindsorHttpDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly IKernel container;

        public WindsorHttpDependencyResolver(IKernel container)
        {
            this.container = container;
        }

        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(container);
        }

        public object GetService(Type serviceType)
        {
            var service = container.HasComponent(serviceType)
                ? container.Resolve(serviceType)
                : null;
            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose()
        {
        }
    }
}