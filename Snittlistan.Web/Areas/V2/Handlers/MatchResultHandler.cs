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
        IEventHandler<MatchResult4Registered>,
        IEventHandler<MatchResult4Updated>,
        IEventHandler<Roster4Changed>,
        IEventHandler<MatchCommentaryEvent>
    {
        public IDocumentSession DocumentSession { get; set; }

        public void Handle(MatchResultRegistered e, string aggregateId)
        {
            DoRegister(aggregateId, e.RosterId, e.TeamScore, e.OpponentScore);
        }

        public void Handle(MatchResultUpdated e, string aggregateId)
        {
            DoUpdated(aggregateId, e.NewRosterId, e.OldBitsMatchId, e.NewBitsMatchId, e.NewTeamScore, e.NewOpponentScore);
        }

        public void Handle(RosterChanged e, string aggregateId)
        {
            DoChanged(aggregateId, e.OldId, e.NewId);
        }

        public void Handle(MatchResult4Registered e, string aggregateId)
        {
            DoRegister(aggregateId, e.RosterId, e.TeamScore, e.OpponentScore);
        }

        public void Handle(MatchResult4Updated e, string aggregateId)
        {
            DoUpdated(aggregateId, e.NewRosterId, e.OldBitsMatchId, e.NewBitsMatchId, e.NewTeamScore, e.NewOpponentScore);
        }

        public void Handle(Roster4Changed e, string aggregateId)
        {
            DoChanged(aggregateId, e.OldId, e.NewId);
        }

        public void Handle(MatchCommentaryEvent e, string aggregateId)
        {
            var id = ResultHeaderReadModel.IdFromBitsMatchId(e.BitsMatchId);
            var results = DocumentSession.Load<ResultHeaderReadModel>(id);
            results.SetMatchCommentary(e.SummaryText, e.BodyText);
        }

        private void DoChanged(string aggregateId, string oldId, string newId)
        {
            var roster = DocumentSession.Load<Roster>(oldId);
            if (roster == null)
            {
                var message = $"Roster {oldId} not found";
                throw new ApplicationException(message);
            }

            roster.MatchResultId = null;
            var newRoster = DocumentSession.Load<Roster>(newId);
            if (newRoster == null)
            {
                var message = $"Roster {newId} not found";
                throw new ApplicationException(message);
            }

            newRoster.MatchResultId = aggregateId;
        }

        private void DoUpdated(string aggregateId, string newRosterId, int oldBitsMatchId, int newBitsMatchId, int newTeamScore, int newOpponentScore)
        {
            var roster = DocumentSession.Load<Roster>(newRosterId);
            if (roster == null) throw new HttpException(404, "Roster not found");

            var id = ResultHeaderReadModel.IdFromBitsMatchId(oldBitsMatchId);
            if (oldBitsMatchId != newBitsMatchId)
            {
                var oldResult = DocumentSession.Load<ResultHeaderReadModel>(id);
                DocumentSession.Delete(oldResult);
                var readModel = new ResultHeaderReadModel(roster, aggregateId, newTeamScore, newOpponentScore);
                DocumentSession.Store(readModel);
            }
            else
            {
                DocumentSession.Load<ResultHeaderReadModel>(id)
                    .SetValues(roster, aggregateId, newTeamScore, newOpponentScore);
            }
        }

        private void DoRegister(string aggregateId, string rosterId, int teamScore, int opponentScore)
        {
            var roster = DocumentSession.Load<Roster>(rosterId);
            if (roster == null) throw new HttpException(404, "Roster not found");

            roster.MatchResultId = aggregateId;
            var id = ResultHeaderReadModel.IdFromBitsMatchId(roster.BitsMatchId);

            var readModel = DocumentSession.Load<ResultHeaderReadModel>(id);
            if (readModel == null)
            {
                readModel = new ResultHeaderReadModel(roster, aggregateId, teamScore, opponentScore);
                DocumentSession.Store(readModel);
            }
            else
            {
                readModel.SetValues(roster, aggregateId, teamScore, opponentScore);
            }
        }
    }
}