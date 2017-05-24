using EventStoreLite;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Handlers
{
    public class SeasonResultsHandler :
        IEventHandler<MatchResultRegistered>,
        IEventHandler<SerieRegistered>
    {
        public IDocumentSession DocumentSession { get; set; }

        public void Handle(MatchResultRegistered e, string aggregateId)
        {
            var roster = DocumentSession.Load<Roster>(e.RosterId);
            var id = SeasonResults.GetId(roster.Season);
            var seasonResults = DocumentSession.Load<SeasonResults>(id);
            if (seasonResults == null)
            {
                seasonResults = new SeasonResults(roster.Season);
                DocumentSession.Store(seasonResults);
            }
        }

        public void Handle(SerieRegistered e, string aggregateId)
        {
            var roster = DocumentSession.Load<Roster>(e.RosterId);
            if (roster.BitsMatchId != 0)
            {
                var id = SeasonResults.GetId(roster.Season);
                var seasonResults = DocumentSession.Load<SeasonResults>(id);
                seasonResults.Add(roster.BitsMatchId, roster.Date, roster.Turn, e.MatchSerie);
            }
        }
    }
}