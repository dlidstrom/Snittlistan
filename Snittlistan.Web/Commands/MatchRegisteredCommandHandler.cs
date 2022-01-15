#nullable enable

using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Commands;

public class MatchRegisteredCommandHandler : CommandHandler<MatchRegisteredCommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(context.Payload.RosterId);
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
            context.Payload.Score,
            context.Payload.OpponentScore,
            resultSeriesReadModel,
            resultHeaderReadModel);
        await CompositionRoot.EmailService.SendAsync(email);
    }

    public record Command(
        string RosterId,
        int BitsMatchId,
        int Score,
        int OpponentScore);
}
