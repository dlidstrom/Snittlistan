#nullable enable

using Raven.Client.Documents.Indexes;

namespace EventStoreLite.Indexes;

public class EventsIndex : AbstractIndexCreationTask<EventStream, EventsIndex.Result>
{
    public EventsIndex()
    {
        Map = streams => from stream in streams
                         from @event in stream.History
                         select new
                         {
                             @event.ChangeSequence,
                             Id = Enumerable.Repeat(new { stream.Id }, 1),
                         };

        Reduce = sequences => from sequence in sequences
                              group sequence by sequence.ChangeSequence into g
                              select new
                              {
                                  ChangeSequence = g.Key,
                                  Id = g.SelectMany(x => x.Id).Distinct()
                              };
    }

    public class StreamId
    {
        public string Id { get; set; } = null!;
    }

    public class Result
    {
        public int ChangeSequence { get; set; }

        public IEnumerable<StreamId> Id { get; set; } = null!;
    }
}
