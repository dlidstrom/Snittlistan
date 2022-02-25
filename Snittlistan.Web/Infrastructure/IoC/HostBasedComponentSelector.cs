#nullable enable

using Castle.MicroKernel;
using Raven.Client;
using Snittlistan.Web.Infrastructure.Database;

namespace Snittlistan.Web.Infrastructure.IoC;

public class HostBasedComponentSelector : IHandlerSelector
{
    public bool HasOpinionAbout(string key, Type service)
    {
        try
        {
            _ = GetHostname();
            bool result = service == typeof(IDocumentStore)
                || service == typeof(Tenant);
            return result;
        }
        catch (Exception)
        {
            return false;
        }
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
