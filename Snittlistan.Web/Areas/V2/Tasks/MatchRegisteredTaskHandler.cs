using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks;
public class MatchRegisteredTaskHandler : TaskHandler<MatchRegisteredTask>
{
    public override async Task Handle(MessageContext<MatchRegisteredTask> context)
    {
        Roster roster = DocumentSession.Load<Roster>(context.Task.RosterId);
        if (roster.IsFourPlayer)
        {
            return;
        }

        string resultSeriesReadModelId = ResultSeriesReadModel.IdFromBitsMatchId(roster.BitsMatchId, roster.Id!);
        ResultSeriesReadModel resultSeriesReadModel = DocumentSession.Load<ResultSeriesReadModel>(resultSeriesReadModelId);
        string resultHeaderReadModelId = ResultHeaderReadModel.IdFromBitsMatchId(roster.BitsMatchId, roster.Id!);
        ResultHeaderReadModel resultHeaderReadModel = DocumentSession.Load<ResultHeaderReadModel>(resultHeaderReadModelId);
        MatchRegisteredEmail email = new(
            roster.Team,
            roster.Opponent!,
            context.Task.Score,
            context.Task.OpponentScore,
            resultSeriesReadModel,
            resultHeaderReadModel);
        await EmailService.SendAsync(email);
    }
}
