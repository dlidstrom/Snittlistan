using System.Web.Http;
using EventStoreLite;
using Raven.Client;
using Snittlistan.Web.Infrastructure.Attributes;

namespace Snittlistan.Web.Controllers
{
    [SaveChanges]
    public abstract class AbstractApiController : ApiController
    {
        /// <summary>
        /// Gets the document store.
        /// </summary>
        public IDocumentStore DocumentStore { get; set; }

        /// <summary>
        /// Gets the document session.
        /// </summary>
        public IDocumentSession DocumentSession { get; set; }

        /// <summary>
        /// Gets the event store session.
        /// </summary>
        public IEventStoreSession EventStoreSession { get; set; }

        /// <summary>
        /// Gets the event store.
        /// </summary>
        public EventStore EventStore { get; set; }

        public void SaveChanges()
        {
            // this commits the document session
            EventStoreSession.SaveChanges();
        }
    }
}