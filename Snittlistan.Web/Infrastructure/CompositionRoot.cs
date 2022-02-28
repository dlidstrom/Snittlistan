#nullable enable

using Castle.MicroKernel;
using EventStoreLite;
using Postal;
using Snittlistan.Web.Infrastructure.Bits;
using Snittlistan.Web.Infrastructure.Database;
using System.Data.Entity;

namespace Snittlistan.Web.Infrastructure;

public record CompositionRoot(
    IKernel Kernel,
    Raven.Client.IDocumentStore DocumentStore,
    Raven.Client.IDocumentSession DocumentSession,
    IEventStoreSession EventStoreSession,
    Databases Databases,
    MsmqFactory MsmqFactory,
    EventStore EventStore,
    Tenant CurrentTenant,
    IEmailService EmailService,
    IBitsClient BitsClient)
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

    public async Task<TenantFeatures?> GetFeatures()
    {
        KeyValueProperty? settingsProperty =
            await Databases.Snittlistan.KeyValueProperties.SingleOrDefaultAsync(
                x => x.Key == TenantFeatures.Key && x.TenantId == CurrentTenant.TenantId);
        return settingsProperty?.Value as TenantFeatures;
    }
}
