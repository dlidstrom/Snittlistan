#nullable enable

using System.Reflection;
using System.Runtime.Serialization;
using EventStoreLite.Infrastructure;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace EventStoreLite;

internal class EventStoreSession : IEventStoreSession
{
    private readonly Dictionary<string, EventStreamAndAggregateRoot> entitiesByKey =
        new();

    private readonly HashSet<EventStreamAndAggregateRoot> unitOfWork
        = new(ObjectReferenceEqualityComparer<object>.Default);

    private readonly IDocumentStore documentStore;
    private readonly IDocumentSession documentSession;
    private readonly EventDispatcher dispatcher;
    private readonly HiLoKeyGenerator eventStreamsHiLoKeyGenerator = new("EventStreams", 4);
    private readonly HiLoKeyGenerator changeSequenceHiLoKeyGenerator = new("ChangeSequence", 4);

    public EventStoreSession(IDocumentStore documentStore, IDocumentSession documentSession, EventDispatcher dispatcher)
    {
        this.documentStore = documentStore ?? throw new ArgumentNullException(nameof(documentStore));
        this.documentSession = documentSession ?? throw new ArgumentNullException(nameof(documentSession));
        this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
    }

    public TAggregate? Load<TAggregate>(string? id) where TAggregate : AggregateRoot
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (entitiesByKey.TryGetValue(id, out EventStreamAndAggregateRoot unitOfWorkInstance))
        {
            return (TAggregate)unitOfWorkInstance.AggregateRoot;
        }

        EventStream stream = documentSession.Load<EventStream>(id);
        if (stream != null)
        {
            TAggregate instance;

            // attempt to call default constructor
            // if none found, create uninitialized object
            ConstructorInfo ctor =
                typeof(TAggregate).GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    Type.EmptyTypes,
                    null);
            if (ctor != null)
            {
                instance = (TAggregate)ctor.Invoke(null);
            }
            else
            {
                instance = (TAggregate)FormatterServices.GetUninitializedObject(typeof(TAggregate));
            }

            instance.LoadFromHistory(stream.History);
            EventStreamAndAggregateRoot eventStreamAndAggregateRoot = new(stream, instance);
            _ = unitOfWork.Add(eventStreamAndAggregateRoot);
            entitiesByKey.Add(id, eventStreamAndAggregateRoot);
            return instance;
        }

        return null;
    }

    public void Store(AggregateRoot aggregate)
    {
        if (aggregate == null)
        {
            throw new ArgumentNullException(nameof(aggregate));
        }

        EventStream eventStream = new();
        GenerateId(eventStream, aggregate);
        documentSession.Store(eventStream);
        aggregate.SetId(eventStream.Id!);
        EventStreamAndAggregateRoot eventStreamAndAggregateRoot = new(eventStream, aggregate);
        _ = unitOfWork.Add(eventStreamAndAggregateRoot);
        entitiesByKey.Add(eventStream.Id!, eventStreamAndAggregateRoot);
    }

    public void SaveChanges()
    {
        var aggregatesAndEvents = from entry in unitOfWork
                                  let aggregateRoot = entry.AggregateRoot
                                  let eventStream = entry.EventStream
                                  from @event in aggregateRoot.GetUncommittedChanges()
                                  orderby @event.TimeStamp
                                  select
                                      new
                                      {
                                          EventStream = eventStream,
                                          Event = @event
                                      };
        Lazy<int> currentChangeSequence = new(GenerateChangeSequence);
        foreach (var aggregatesAndEvent in aggregatesAndEvents)
        {
            IDomainEvent pendingEvent = aggregatesAndEvent.Event;
            EventStream eventStream = aggregatesAndEvent.EventStream;
            dynamic? asDynamic = pendingEvent.AsDynamic();
            asDynamic!.SetChangeSequence(currentChangeSequence.Value);
            dispatcher.Dispatch(pendingEvent, eventStream.Id!);
            eventStream.History.Add(pendingEvent);
        }

        foreach (AggregateRoot aggregateRoot in unitOfWork.Select(x => x.AggregateRoot))
        {
            aggregateRoot.ClearUncommittedChanges();
        }

        documentSession.SaveChanges();
    }

    private void GenerateId(EventStream eventStream, AggregateRoot aggregate)
    {
        string typeTagName = documentStore.Conventions.GetTypeTagName(aggregate.GetType());
        string id = eventStreamsHiLoKeyGenerator.GenerateDocumentKey(
            documentStore.DatabaseCommands, documentStore.Conventions, eventStream);
        string identityPartsSeparator = documentStore.Conventions.IdentityPartsSeparator;
        int lastIndexOf = id.LastIndexOf(identityPartsSeparator, StringComparison.Ordinal);
        eventStream.Id = string.Format(
            "EventStreams{2}{0}{2}{1}", typeTagName, id.Substring(lastIndexOf + 1), identityPartsSeparator);
    }

    private int GenerateChangeSequence()
    {
        string id = changeSequenceHiLoKeyGenerator.GenerateDocumentKey(
            documentStore.DatabaseCommands, documentStore.Conventions, null);
        string identityPartsSeparator = documentStore.Conventions.IdentityPartsSeparator;
        int lastIndexOf = id.LastIndexOf(identityPartsSeparator, StringComparison.Ordinal);
        return int.Parse(id.Substring(lastIndexOf + 1));
    }
}
