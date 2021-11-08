#nullable enable

namespace Snittlistan.Web.Areas.V2.Handlers
{
    using System.Web;
    using EventStoreLite;
    using Raven.Client;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Domain.Match.Events;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public class MatchResultHandler :
        IEventHandler<MatchResultRegistered>,
        IEventHandler<MatchResult4Registered>,
        IEventHandler<MatchCommentaryEvent>
    {
        public IDocumentSession DocumentSession { get; set; } = null!;

        public void Handle(MatchResultRegistered e, string aggregateId)
        {
            DoRegister(aggregateId, e.RosterId, e.TeamScore, e.OpponentScore);
        }

        public void Handle(MatchResult4Registered e, string aggregateId)
        {
            DoRegister(aggregateId, e.RosterId, e.TeamScore, e.OpponentScore);
        }

        public void Handle(MatchCommentaryEvent e, string aggregateId)
        {
            string id = ResultHeaderReadModel.IdFromBitsMatchId(e.BitsMatchId, e.RosterId);
            ResultHeaderReadModel results = DocumentSession.Load<ResultHeaderReadModel>(id);
            results.SetMatchCommentary(e.SummaryText, e.SummaryHtml, e.BodyText);
        }

        private void DoRegister(string aggregateId, string rosterId, int teamScore, int opponentScore)
        {
            Roster roster = DocumentSession.Load<Roster>(rosterId);
            if (roster == null)
            {
                throw new HttpException(404, "Roster not found");
            }

            roster.MatchResultId = aggregateId;
            string id = ResultHeaderReadModel.IdFromBitsMatchId(roster.BitsMatchId, roster.Id!);

            ResultHeaderReadModel readModel = DocumentSession.Load<ResultHeaderReadModel>(id);
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
