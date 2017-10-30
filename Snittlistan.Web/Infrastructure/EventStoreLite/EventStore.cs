using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.Windsor;
using EventStoreLite.Indexes;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Linq;

// ReSharper disable once CheckNamespace
namespace EventStoreLite
{
    /// <summary>
    /// Represents the event store. Use this class to create event store sessions.
    /// Typically, an instance of this class should be a singleton in your application.
    /// </summary>
    public class EventStore
    {
        private static readonly object InitLock = new object();
        private static bool initialized;
        private readonly IWindsorContainer container;
        private IEnumerable<Type> readModelTypes;

        internal EventStore(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));
            this.container = container;
        }

        /// <summary>
        /// Rebuilds all read models. This is a potentially lengthy operation!
        /// </summary>
        public static void ReplayEvents(IWindsorContainer container)
        {
            if (!initialized) throw new InvalidOperationException("The event store must be initialized before first usage.");
            IDocumentStore documentStore = null;
            try
            {
                documentStore = container.Resolve<IDocumentStore>();
                DoReplayEvents(container, documentStore);
            }
            finally
            {
                if (documentStore != null)
                    container.Release(documentStore);
            }
        }

        /// <summary>
        /// Initialize the event store. This will create the necessary
        /// indexes. This method can be called several times.
        /// </summary>
        /// <param name="documentStore">Document store.</param>
        /// <returns>Event store instance.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public EventStore Initialize(IDocumentStore documentStore)
        {
            if (documentStore == null) throw new ArgumentNullException(nameof(documentStore));
            DoInitialize(documentStore);
            return this;
        }

        /// <summary>
        /// Opens a new event store session.
        /// </summary>
        /// <param name="documentStore">Document store.</param>
        /// <param name="session">Document session.</param>
        /// <returns>Event store session.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public IEventStoreSession OpenSession(IDocumentStore documentStore, IDocumentSession session)
        {
            if (!initialized) throw new InvalidOperationException("The event store must be initialized before first usage.");
            if (documentStore == null) throw new ArgumentNullException(nameof(documentStore));
            if (session == null) throw new ArgumentNullException(nameof(session));

            return new EventStoreSession(documentStore, session, new EventDispatcher(container));
        }

        /// <summary>
        /// Migrates all store events. This will simply pass all events from all aggregates
        /// into the specified migrators. There is a chance of events coming in the wrong
        /// order, so don't rely on them coming in the order they were raised. They will,
        /// however, come in the correct order for each individual aggregate.
        /// </summary>
        /// <param name="eventMigrators">Event migrators.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void MigrateEvents(IEnumerable<IEventMigrator> eventMigrators)
        {
            if (!initialized) throw new InvalidOperationException("The event store must be initialized before first usage.");
            if (eventMigrators == null) throw new ArgumentNullException(nameof(eventMigrators));

            // order by defined date
            eventMigrators = eventMigrators.OrderBy(x => x.DefinedOn()).ToList();

            var current = 0;
            while (true)
            {
                var session = (IDocumentSession)container.Resolve(typeof(IDocumentSession));
                try
                {
                    // allow indexing to take its time
                    var q =
                        session.Query<EventStream>()
                               .Customize(x => x.WaitForNonStaleResultsAsOf(DateTime.Now.AddSeconds(15)));

                    var eventStreams = q.Skip(current).Take(128).ToList();
                    if (eventStreams.Count == 0) break;
                    foreach (var eventStream in eventStreams)
                    {
                        var newHistory = new List<IDomainEvent>();
                        foreach (var domainEvent in eventStream.History)
                        {
                            var oldEvents = new List<IDomainEvent> { domainEvent };
                            foreach (var eventMigrator in eventMigrators)
                            {
                                var newEvents = new List<IDomainEvent>();
                                foreach (var migratedEvent in oldEvents)
                                {
                                    newEvents.AddRange(eventMigrator.Migrate(migratedEvent, eventStream.Id));
                                }

                                oldEvents = newEvents;
                            }

                            newHistory.AddRange(oldEvents);
                        }

                        eventStream.History = newHistory;
                    }

                    session.SaveChanges();
                    current += eventStreams.Count;
                }
                finally
                {
                    container.Release(session);
                }
            }
        }

        internal EventStore SetReadModelTypes(IEnumerable<Type> types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));
            readModelTypes = types;

            return this;
        }

        private static void DoReplayEvents(IWindsorContainer container, IDocumentStore documentStore)
        {
            // wait for indexing to complete
            WaitForIndexing(documentStore);

            // delete all read models
            documentStore.DatabaseCommands.DeleteByIndex("ReadModelIndex", new IndexQuery());

            // load all event streams and dispatch events
            var dispatcher = new EventDispatcher(container);

            var current = 0;
            while (true)
            {
                IDocumentSession session = null;

                try
                {
                    session = (IDocumentSession)container.Resolve(typeof(IDocumentSession));
                    var eventsQuery =
                        session.Query<EventsIndex.Result, EventsIndex>()
                               .Customize(
                                   x => x.WaitForNonStaleResultsAsOf(DateTime.Now.AddSeconds(15)))
                               .OrderBy(x => x.ChangeSequence);
                    var results = eventsQuery.Skip(current).Take(128).ToList();
                    if (results.Count == 0) break;
                    foreach (var result in results)
                    {
                        var changeSequence = result.ChangeSequence;
                        var ids = result.Id.Select(x => x.Id);
                        var streams = session.Load<EventStream>(ids);

                        var events = from stream in streams
                                     from @event in stream.History
                                     where @event.ChangeSequence == changeSequence
                                     orderby @event.TimeStamp
                                     select new { stream.Id, Event = @event };
                        foreach (var item in events)
                        {
                            dispatcher.Dispatch(item.Event, item.Id);
                        }
                    }

                    session.SaveChanges();
                    current += results.Count;
                }
                finally
                {
                    if (session != null)
                        container.Release(session);
                }
            }
        }

        private static void WaitForIndexing(IDocumentStore documentStore)
        {
            var indexingTask = Task.Factory.StartNew(
                () =>
                {
                    while (true)
                    {
                        var s = documentStore.DatabaseCommands.GetStatistics().StaleIndexes;
                        if (!s.Contains("ReadModelIndex"))
                        {
                            break;
                        }
                        Thread.Sleep(500);
                    }
                });
            indexingTask.Wait(15000);
        }

        private void DoInitialize(IDocumentStore documentStore)
        {
            lock (InitLock)
            {
                new ReadModelIndex(readModelTypes).Execute(documentStore);
                new EventsIndex().Execute(documentStore);
                initialized = true;
            }
        }
    }
}