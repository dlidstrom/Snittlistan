using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Infrastructure.BackgroundTasks;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Tasks
{
    public class MatchRegisteredTaskHandler : BackgroundTaskHandler<MatchRegisteredTask>
    {
        public override void Handle(MatchRegisteredTask task)
        {
            Transact(session =>
            {
                var roster = session.Load<Roster>(task.RosterId);
                if (roster.IsFourPlayer) return;
                var resultSeriesReadModelId = ResultSeriesReadModel.IdFromBitsMatchId(roster.BitsMatchId);
                var resultSeriesReadModel = session.Load<ResultSeriesReadModel>(resultSeriesReadModelId);
                var resultHeaderReadModelId = ResultHeaderReadModel.IdFromBitsMatchId(roster.BitsMatchId);
                var resultHeaderReadModel = session.Load<ResultHeaderReadModel>(resultHeaderReadModelId);
                Emails.MatchRegistered(
                    roster.Team,
                    roster.Opponent,
                    task.Score,
                    task.OpponentScore,
                    resultSeriesReadModel,
                    resultHeaderReadModel);
            });
        }
    }
}