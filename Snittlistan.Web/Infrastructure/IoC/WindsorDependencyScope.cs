#nullable enable

namespace Snittlistan.Web.Infrastructure.IoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Dependencies;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Lifestyle;

    public sealed class WindsorDependencyScope : IDependencyScope
    {
        private readonly IKernel container;
        private readonly IDisposable scope;

        public WindsorDependencyScope(IKernel container)
        {
            this.container = container;
            scope = container.BeginScope();
        }

        public object? GetService(Type serviceType)
        {
            object? service = container.HasComponent(serviceType) ? container.Resolve(serviceType) : null;
            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}
