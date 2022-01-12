#nullable enable

using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Areas.V2.Tasks;

public class MatchRegisteredTaskHandler : TaskHandler<MatchRegisteredTask>
{
    public override async Task Handle(MessageContext<MatchRegisteredTask> context)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(context.Task.RosterId);
        if (roster.IsFourPlayer)
        {
            return;
        }

        string resultSeriesReadModelId = ResultSeriesReadModel.IdFromBitsMatchId(roster.BitsMatchId, roster.Id!);
        ResultSeriesReadModel resultSeriesReadModel = CompositionRoot.DocumentSession.Load<ResultSeriesReadModel>(resultSeriesReadModelId);
        string resultHeaderReadModelId = ResultHeaderReadModel.IdFromBitsMatchId(roster.BitsMatchId, roster.Id!);
        ResultHeaderReadModel resultHeaderReadModel = CompositionRoot.DocumentSession.Load<ResultHeaderReadModel>(resultHeaderReadModelId);
        MatchRegisteredEmail email = new(
            roster.Team,
            roster.Opponent!,
            context.Task.Score,
            context.Task.OpponentScore,
            resultSeriesReadModel,
            resultHeaderReadModel);
        await CompositionRoot.EmailService.SendAsync(email);
    }
}
