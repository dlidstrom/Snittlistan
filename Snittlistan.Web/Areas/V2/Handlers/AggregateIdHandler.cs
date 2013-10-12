using EventStoreLite;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Handlers
{
    public class AggregateIdHandler :
        IEventHandler<MatchResultRegistered>,
        IEventHandler<MatchResult4Registered>
    {
        public IDocumentSession DocumentSession { get; set; }

        public void Handle(MatchResultRegistered e, string aggregateId)
        {
            DocumentSession.Store(new AggregateIdReadModel(aggregateId, typeof(MatchResult)));
        }

        public void Handle(MatchResult4Registered e, string aggregateId)
        {
            DocumentSession.Store(new AggregateIdReadModel(aggregateId, typeof(MatchResult4)));
        }
    }
}