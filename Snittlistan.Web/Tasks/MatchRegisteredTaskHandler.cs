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
                var id = ResultSeriesReadModel.IdFromBitsMatchId(roster.BitsMatchId);
                var resultSeriesReadModel = session.Load<ResultSeriesReadModel>(id);
                Emails.MatchRegistered(
                    roster.Team,
                    roster.Opponent,
                    task.Score,
                    task.OpponentScore,
                    resultSeriesReadModel);
            });
        }
    }
}