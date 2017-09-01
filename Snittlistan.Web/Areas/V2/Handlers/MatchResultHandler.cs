using System;
using System.Web;
using EventStoreLite;
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match.Events;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.Handlers
{
    public class MatchResultHandler :
        IEventHandler<MatchResultRegistered>,
        IEventHandler<MatchResultUpdated>,
        IEventHandler<RosterChanged>,
        IEventHandler<MatchResultDeleted>,
        IEventHandler<MatchResult4Registered>,
        IEventHandler<MatchResult4Updated>,
        IEventHandler<Roster4Changed>,
        IEventHandler<MatchResult4Deleted>,
        IEventHandler<MatchCommentaryEvent>
    {
        public IDocumentSession DocumentSession { get; set; }

        public void Handle(MatchResultRegistered e, string aggregateId)
        {
            DoRegister(aggregateId, e.RosterId, e.TeamScore, e.OpponentScore, e.BitsMatchId, false);
        }

        public void Handle(MatchResultUpdated e, string aggregateId)
        {
            DoUpdated(aggregateId, e.NewRosterId, e.OldBitsMatchId, e.NewBitsMatchId, e.NewTeamScore, e.NewOpponentScore, false);
        }

        public void Handle(RosterChanged e, string aggregateId)
        {
            DoChanged(aggregateId, e.OldId, e.NewId);
        }

        public void Handle(MatchResultDeleted e, string aggregateId)
        {
            DoDeleted(e.RosterId, e.BitsMatchId);
        }

        public void Handle(MatchResult4Registered e, string aggregateId)
        {
            DoRegister(aggregateId, e.RosterId, e.TeamScore, e.OpponentScore, e.BitsMatchId, true);
        }

        public void Handle(MatchResult4Updated e, string aggregateId)
        {
            DoUpdated(aggregateId, e.NewRosterId, e.OldBitsMatchId, e.NewBitsMatchId, e.NewTeamScore, e.NewOpponentScore, true);
        }

        public void Handle(Roster4Changed e, string aggregateId)
        {
            DoChanged(aggregateId, e.OldId, e.NewId);
        }

        public void Handle(MatchResult4Deleted e, string aggregateId)
        {
            DoDeleted(e.RosterId, e.BitsMatchId);
        }

        public void Handle(MatchCommentaryEvent e, string aggregateId)
        {
            var id = ResultHeaderReadModel.IdFromBitsMatchId(e.BitsMatchId);
            var results = DocumentSession.Load<ResultHeaderReadModel>(id);
            results.SetMatchCommentary(e.SummaryText, e.BodyText);
        }

        private void DoDeleted(string rosterId, int bitsMatchId)
        {
            var roster = DocumentSession.Load<Roster>(rosterId);
            if (roster == null) throw new HttpException(404, "Roster not found");
            roster.MatchResultId = null;
            var result = DocumentSession.Load<ResultHeaderReadModel>(ResultHeaderReadModel.IdFromBitsMatchId(bitsMatchId));
            DocumentSession.Delete(result);
        }

        private void DoChanged(string aggregateId, string oldId, string newId)
        {
            var roster = DocumentSession.Load<Roster>(oldId);
            if (roster == null)
            {
                var message = string.Format("Roster {0} not found", oldId);
                throw new ApplicationException(message);
            }

            roster.MatchResultId = null;
            var newRoster = DocumentSession.Load<Roster>(newId);
            if (newRoster == null)
            {
                var message = string.Format("Roster {0} not found", newId);
                throw new ApplicationException(message);
            }

            newRoster.MatchResultId = aggregateId;
        }

        private void DoUpdated(string aggregateId, string newRosterId, int oldBitsMatchId, int newBitsMatchId, int newTeamScore, int newOpponentScore, bool isFourPlayer)
        {
            var roster = DocumentSession.Load<Roster>(newRosterId);
            if (roster == null) throw new HttpException(404, "Roster not found");

            var id = ResultHeaderReadModel.IdFromBitsMatchId(oldBitsMatchId);
            if (oldBitsMatchId != newBitsMatchId)
            {
                var oldResult = DocumentSession.Load<ResultHeaderReadModel>(id);
                DocumentSession.Delete(oldResult);
                var readModel = new ResultHeaderReadModel(
                    roster, aggregateId, newTeamScore, newOpponentScore, newBitsMatchId, isFourPlayer);
                DocumentSession.Store(readModel);
            }
            else
            {
                DocumentSession.Load<ResultHeaderReadModel>(id)
                    .SetValues(roster, aggregateId, newTeamScore, newOpponentScore, newBitsMatchId, isFourPlayer);
            }
        }

        private void DoRegister(string aggregateId, string rosterId, int teamScore, int opponentScore, int bitsMatchId, bool isFourPlayer)
        {
            var roster = DocumentSession.Load<Roster>(rosterId);
            if (roster == null) throw new HttpException(404, "Roster not found");

            roster.MatchResultId = aggregateId;
            var readModel = new ResultHeaderReadModel(roster, aggregateId, teamScore, opponentScore, bitsMatchId, isFourPlayer);
            DocumentSession.Store(readModel);
        }
    }
}