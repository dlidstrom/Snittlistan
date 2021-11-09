#nullable enable

namespace EventStoreLite
{
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

    /// <summary>
    /// Represents the event store. Use this class to create event store sessions.
    /// Typically, an instance of this class should be a singleton in your application.
    /// </summary>
    public class EventStore
    {
        private static readonly object InitLock = new();
        private static bool initialized;
        private readonly IWindsorContainer container;
        private IEnumerable<Type>? readModelTypes;

        internal EventStore(IWindsorContainer container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <summary>
        /// Rebuilds all read models. This is a potentially lengthy operation!
        /// </summary>
        public static void ReplayEvents(IWindsorContainer container)
        {
            if (!initialized)
            {
                throw new InvalidOperationException("The event store must be initialized before first usage.");
            }

            IDocumentStore? documentStore = null;
            try
            {
                documentStore = container.Resolve<IDocumentStore>();
                DoReplayEvents(container, documentStore);
            }
            finally
            {
                if (documentStore != null)
                {
                    container.Release(documentStore);
                }
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
            if (documentStore == null)
            {
                throw new ArgumentNullException(nameof(documentStore));
            }

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
            if (!initialized)
            {
                throw new InvalidOperationException("The event store must be initialized before first usage.");
            }

            if (documentStore == null)
            {
                throw new ArgumentNullException(nameof(documentStore));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

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
            if (!initialized)
            {
                throw new InvalidOperationException("The event store must be initialized before first usage.");
            }

            if (eventMigrators == null)
            {
                throw new ArgumentNullException(nameof(eventMigrators));
            }

            // order by defined date
            eventMigrators = eventMigrators.OrderBy(x => x.DefinedOn()).ToList();

            int current = 0;
            while (true)
            {
                IDocumentSession session = (IDocumentSession)container.Resolve(typeof(IDocumentSession));
                try
                {
                    // allow indexing to take its time
                    IRavenQueryable<EventStream> q =
                        session.Query<EventStream>()
                               .Customize(x => x.WaitForNonStaleResultsAsOf(DateTime.Now.AddSeconds(15)));

                    List<EventStream> eventStreams = q.Skip(current).Take(128).ToList();
                    if (eventStreams.Count == 0)
                    {
                        break;
                    }

                    foreach (EventStream eventStream in eventStreams)
                    {
                        List<IDomainEvent> newHistory = new();
                        foreach (IDomainEvent domainEvent in eventStream.History)
                        {
                            List<IDomainEvent> oldEvents = new() { domainEvent };
                            foreach (IEventMigrator eventMigrator in eventMigrators)
                            {
                                List<IDomainEvent> newEvents = new();
                                foreach (IDomainEvent migratedEvent in oldEvents)
                                {
                                    newEvents.AddRange(eventMigrator.Migrate(migratedEvent, eventStream.Id!));
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
            readModelTypes = types ?? throw new ArgumentNullException(nameof(types));

            return this;
        }

        private static void DoReplayEvents(IWindsorContainer container, IDocumentStore documentStore)
        {
            // wait for indexing to complete
            WaitForIndexing(documentStore);

            // delete all read models
            _ = documentStore.DatabaseCommands.DeleteByIndex("ReadModelIndex", new IndexQuery());

            // load all event streams and dispatch events
            EventDispatcher dispatcher = new(container);

            int current = 0;
            while (true)
            {
                IDocumentSession? session = null;

                try
                {
                    session = (IDocumentSession)container.Resolve(typeof(IDocumentSession));
                    IRavenQueryable<EventsIndex.Result> eventsQuery =
                        session.Query<EventsIndex.Result, EventsIndex>()
                               .Customize(
                                   x => x.WaitForNonStaleResultsAsOf(DateTime.Now.AddSeconds(15)))
                               .OrderBy(x => x.ChangeSequence);
                    List<EventsIndex.Result> results = eventsQuery.Skip(current).Take(128).ToList();
                    if (results.Count == 0)
                    {
                        break;
                    }

                    foreach (EventsIndex.Result result in results)
                    {
                        int changeSequence = result.ChangeSequence;
                        IEnumerable<string> ids = result.Id.Select(x => x.Id);
                        EventStream[] streams = session.Load<EventStream>(ids);

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
                    {
                        container.Release(session);
                    }
                }
            }
        }

        private static void WaitForIndexing(IDocumentStore documentStore)
        {
            Task indexingTask = Task.Factory.StartNew(
                () =>
                {
                    while (true)
                    {
                        string[] s = documentStore.DatabaseCommands.GetStatistics().StaleIndexes;
                        if (!s.Contains("ReadModelIndex"))
                        {
                            break;
                        }
                        Thread.Sleep(500);
                    }
                });
            _ = indexingTask.Wait(15000);
        }

        private void DoInitialize(IDocumentStore documentStore)
        {
            lock (InitLock)
            {
                new ReadModelIndex(readModelTypes!).Execute(documentStore);
                new EventsIndex().Execute(documentStore);
                initialized = true;
            }
        }
    }
}
