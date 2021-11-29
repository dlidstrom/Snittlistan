#nullable enable

namespace Snittlistan.Web.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Castle.MicroKernel;
    using EventStoreLite;
    using Snittlistan.Queue;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Attributes;
    using Snittlistan.Web.Infrastructure.Database;

    [SaveChanges]
    public abstract class AbstractApiController : ApiController
    {
        public IKernel Kernel { get; set; } = null!;

        public Raven.Client.IDocumentStore DocumentStore { get; set; } = null!;

        public Raven.Client.IDocumentSession DocumentSession { get; set; } = null!;

        public IEventStoreSession EventStoreSession { get; set; } = null!;

        public Databases Databases { get; set; } = null!;

        public EventStore EventStore { get; set; } = null!;

        public IMsmqTransaction MsmqTransaction { get; set; } = null!;

        [NonAction]
        public async Task SaveChangesAsync()
        {
            MsmqTransaction.Commit();

            // this commits the document session
            EventStoreSession.SaveChanges();

            _ = await Databases.Snittlistan.SaveChangesAsync();
        }

        protected async Task<Tenant> GetCurrentTenant()
        {
            string hostname = CurrentHttpContext.Instance().Request.ServerVariables["SERVER_NAME"];
            Tenant tenant = await Databases.Snittlistan.Tenants.SingleOrDefaultAsync(x => x.Hostname == hostname);
            if (tenant == null)
            {
                throw new Exception($"No tenant found for hostname '{hostname}'");
            }

            return tenant;
        }
    }
}
