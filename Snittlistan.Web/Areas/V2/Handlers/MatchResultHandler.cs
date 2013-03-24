using System.Web;
using EventStoreLite;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Handlers
{
    public class MatchResultHandler : IEventHandler<MatchResultRegistered>,
        IEventHandler<MatchResultUpdated>,
        IEventHandler<RosterChanged>,
        IEventHandler<MatchResultDeleted>
    {
        public IDocumentSession DocumentSession { get; set; }

        public void Handle(MatchResultRegistered e, string aggregateId)
        {
            var roster = DocumentSession.Load<Roster>(e.RosterId);
            if (roster == null) throw new HttpException(404, "Roster not found");

            var readModel = new ResultHeaderReadModel(roster, aggregateId, e.TeamScore, e.OpponentScore, e.BitsMatchId);
            DocumentSession.Store(readModel);
        }

        public void Handle(MatchResultUpdated e, string aggregateId)
        {
            var roster = DocumentSession.Load<Roster>(e.NewRosterId);
            if (roster == null) throw new HttpException(404, "Roster not found");

            var id = ResultHeaderReadModel.IdFromBitsMatchId(e.OldBitsMatchId);
            if (e.OldBitsMatchId != e.NewBitsMatchId)
            {
                var oldResult = DocumentSession.Load<ResultHeaderReadModel>(id);
                DocumentSession.Delete(oldResult);
                var readModel = new ResultHeaderReadModel(
                    roster, aggregateId, e.NewTeamScore, e.NewOpponentScore, e.NewBitsMatchId);
                DocumentSession.Store(readModel);
            }
            else
            {
                DocumentSession.Load<ResultHeaderReadModel>(id)
                    .SetValues(roster, aggregateId, e.NewTeamScore, e.NewOpponentScore, e.NewBitsMatchId);
            }
        }

        public void Handle(RosterChanged e, string aggregateId)
        {
            var roster = DocumentSession.Load<Roster>(e.OldId);
            if (roster == null) throw new HttpException(404, "Roster not found");
            roster.MatchResultId = null;
        }

        public void Handle(MatchResultDeleted e, string aggregateId)
        {
            var roster = DocumentSession.Load<Roster>(e.RosterId);
            if (roster == null) throw new HttpException(404, "Roster not found");
            roster.MatchResultId = null;
            var result = DocumentSession.Load<ResultHeaderReadModel>(ResultHeaderReadModel.IdFromBitsMatchId(e.BitsMatchId));
            DocumentSession.Delete(result);
        }
    }
}