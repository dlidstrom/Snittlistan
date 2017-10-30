using System;
using System.Linq;
using System.Web;
using Castle.MicroKernel;
using Raven.Client;

namespace Snittlistan.Web.Infrastructure.IoC
{
    public class HostBasedComponentSelector : IHandlerSelector
    {
        public bool HasOpinionAbout(string key, Type service)
        {
            try
            {
                GetHostname();
                var result = service == typeof(TenantConfiguration)
                    || service == typeof(IDocumentStore);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IHandler SelectHandler(string key, Type service, IHandler[] handlers)
        {
            var hostname = GetHostname();
            if (service == typeof(IDocumentStore))
            {
                hostname = $"DocumentStore-{hostname}";
            }

            var selectedHandler = handlers.FirstOrDefault(h => h.ComponentModel.Name == hostname)
                                  ?? GetDefaultHandler(service, handlers);
            return selectedHandler;
        }

        private static IHandler GetDefaultHandler(Type service, IHandler[] handlers)
        {
            if (handlers.Length == 0)
            {
                throw new ApplicationException($"No components registered for service {service.Name}");
            }

            return handlers[0];
        }

        private static string GetHostname()
        {
            return HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
        }
    }
}