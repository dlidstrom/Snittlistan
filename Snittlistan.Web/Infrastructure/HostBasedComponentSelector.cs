using System;
using System.Linq;
using System.Web;
using Castle.MicroKernel;

namespace Snittlistan.Web.Infrastructure
{
    public class HostBasedComponentSelector : IHandlerSelector
    {
        public bool HasOpinionAbout(string key, Type service)
        {
            return service == typeof(TenantConfiguration);
        }

        public IHandler SelectHandler(string key, Type service, IHandler[] handlers)
        {
            var id = GetHostname();
            var selectedHandler = handlers.FirstOrDefault(h => h.ComponentModel.Name == id)
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