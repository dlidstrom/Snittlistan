#nullable enable

namespace Snittlistan.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using EventStoreLite;
    using Raven.Client;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Models;
    using Snittlistan.Web.Infrastructure.Attributes;
    using Snittlistan.Web.Infrastructure.Database;

    [SaveChanges]
    public abstract class AbstractApiController : ApiController
    {
        public IDocumentStore DocumentStore { get; set; } = null!;

        public IDocumentSession DocumentSession { get; set; } = null!;

        public IEventStoreSession EventStoreSession { get; set; } = null!;

        public Databases Databases { get; set; } = null!;

        public EventStore EventStore { get; set; } = null!;

        public TenantConfiguration TenantConfiguration { get; set; } = null!;

        public IMsmqTransaction MsmqTransaction { get; set; } = null!;

        [NonAction]
        public async Task SaveChangesAsync()
        {
            MsmqTransaction.Commit();

            // this commits the document session
            EventStoreSession.SaveChanges();

            _ = await Databases.Snittlistan.SaveChangesAsync();
        }
    }
}
