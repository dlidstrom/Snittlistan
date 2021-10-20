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
        public IDocumentStore DocumentStore { get; set; }

        public IDocumentSession DocumentSession { get; set; }

        public IEventStoreSession EventStoreSession { get; set; }

        public DatabaseContext Database { get; set; }

        public EventStore EventStore { get; set; }

        public TenantConfiguration TenantConfiguration { get; set; }

        public IMsmqTransaction MsmqTransaction { get; set; }

        [NonAction]
        public async Task SaveChangesAsync()
        {
            MsmqTransaction.Commit();

            // this commits the document session
            EventStoreSession.SaveChanges();

            _ = await Database.SaveChangesAsync();
        }
    }
}
