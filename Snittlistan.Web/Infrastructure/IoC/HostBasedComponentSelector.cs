#nullable enable

namespace Snittlistan.Web.Infrastructure.IoC
{
    using System;
    using System.Linq;
    using Castle.MicroKernel;
    using Raven.Client;

    public class HostBasedComponentSelector : IHandlerSelector
    {
        public bool HasOpinionAbout(string key, Type service)
        {
            bool result = service == typeof(IDocumentStore);
            return result;
        }

        public IHandler SelectHandler(string key, Type service, IHandler[] handlers)
        {
            string hostname = GetHostname();
            if (service == typeof(IDocumentStore))
            {
                hostname = $"DocumentStore-{hostname}";
            }

            IHandler selectedHandler = handlers.SingleOrDefault(h => h.ComponentModel.Name == hostname);
            if (selectedHandler == null)
            {
                throw new Exception($"No {service} configured with name {hostname}");
            }

            return selectedHandler;
        }

        private static string GetHostname()
        {
            return CurrentHttpContext.Instance().Request.ServerVariables["SERVER_NAME"];
        }
    }
}
