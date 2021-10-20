﻿#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System.Threading.Tasks;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.ReadModels;
    using Snittlistan.Web.Models;

    public class MatchRegisteredTaskHandler : TaskHandler<MatchRegisteredTask>
    {
        public override async Task Handle(MatchRegisteredTask task)
        {
            Roster roster = DocumentSession.Load<Roster>(task.RosterId);
            if (roster.IsFourPlayer)
            {
                return;
            }

            string resultSeriesReadModelId = ResultSeriesReadModel.IdFromBitsMatchId(roster.BitsMatchId, roster.Id);
            ResultSeriesReadModel resultSeriesReadModel = DocumentSession.Load<ResultSeriesReadModel>(resultSeriesReadModelId);
            string resultHeaderReadModelId = ResultHeaderReadModel.IdFromBitsMatchId(roster.BitsMatchId, roster.Id);
            ResultHeaderReadModel resultHeaderReadModel = DocumentSession.Load<ResultHeaderReadModel>(resultHeaderReadModelId);
            MatchRegisteredEmail email = new(
                roster.Team,
                roster.Opponent,
                task.Score,
                task.OpponentScore,
                resultSeriesReadModel,
                resultHeaderReadModel);
            await EmailService.SendAsync(email);
        }
    }
}