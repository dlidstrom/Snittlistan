namespace Snittlistan.Web.Controllers
{
    using System;
    using System.Web.Http;
    using EventStoreLite;
    using Raven.Client;
    using Snittlistan.Queue;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Infrastructure.Attributes;

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

        /// <summary>
        /// Gets the msmq transaction.
        /// </summary>
        public IMsmqTransaction MsmqTransaction { get; set; }

        [NonAction]
        public void SaveChanges()
        {
            MsmqTransaction.Commit();

            // this commits the document session
            EventStoreSession.SaveChanges();
        }

        protected TResult ExecuteQuery<TResult>(IQuery<TResult> query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            return query.Execute(DocumentSession);
        }

        protected void ExecuteCommand(ICommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            command.Execute(DocumentSession, EventStoreSession, PublishMessage);
        }

        protected void PublishMessage<TPayload>(TPayload payload)
        {
            string uriString = Url.Link("DefaultApi", new { controller = "Task" });
            var uri = new Uri(uriString);
            var envelope = new MessageEnvelope(payload, uri);
            MsmqTransaction.PublishMessage(envelope);
        }
    }
}
