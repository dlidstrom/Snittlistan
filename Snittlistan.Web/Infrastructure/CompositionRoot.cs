#nullable enable

using Castle.Core.Logging;
using Castle.MicroKernel;
using EventStoreLite;
using Postal;
using Snittlistan.Web.Infrastructure.Bits;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Infrastructure;

public record CompositionRoot(
    IKernel Kernel,
    Raven.Client.IDocumentStore DocumentStore,
    Raven.Client.IDocumentSession DocumentSession,
    IEventStoreSession EventStoreSession,
    Databases Databases,
    DatabasesFactory DatabasesFactory,
    MsmqFactory MsmqFactory,
    EventStore EventStore,
    Tenant CurrentTenant,
    IEmailService EmailService,
    IBitsClient BitsClient,
    ILogger Logger)
{
    public Guid CorrelationId
    {
        get
        {
            if (CurrentHttpContext.Instance().Items["CorrelationId"] is Guid correlationId)
            {
                return correlationId;
            }

            correlationId = Guid.NewGuid();
            CurrentHttpContext.Instance().Items["CorrelationId"] = correlationId;
            return correlationId;
        }
    }

    public async Task<TenantFeatures> GetFeatures()
    {
        KeyValueProperty? settingsProperty =
            await Databases.Snittlistan.KeyValueProperties.SingleOrDefaultAsync(
                x => x.Key == TenantFeatures.Key && x.TenantId == CurrentTenant.TenantId);
        do
        {
            if (settingsProperty is null)
            {
                Logger.Info("no tenant features found");
                break;
            }

            if (settingsProperty.Value is TenantFeatures tenantFeatures)
            {
                Logger.InfoFormat("found tenant features {@tenantFeatures}", tenantFeatures);
                return tenantFeatures;
            }
        }
        while (false);

        Logger.Warn("no tenant features found, or unusable");
        return TenantFeatures.Default;
    }
}
