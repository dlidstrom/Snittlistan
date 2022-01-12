#nullable enable

using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;

namespace Snittlistan.Web.Commands;

public class RegisterMatch4CommandHandler : CommandHandler<RegisterMatch4CommandHandler.Command>
{
    public override Task Handle(CommandContext<Command> context)
    {
        Roster roster = CompositionRoot.DocumentSession.Load<Roster>(context.Command.RosterId);
        MatchResult4 matchResult = new(
            roster,
            context.Command.Result.TeamScore,
            context.Command.Result.OpponentScore,
            roster.BitsMatchId);
        Player[] players = CompositionRoot.DocumentSession.Load<Player>(roster.Players);

        MatchSerie4[] matchSeries = context.Command.Result.CreateMatchSeries();
        matchResult.RegisterSeries(
            task => context.PublishMessage(task),
            matchSeries,
            players,
            context.Command.SummaryText ?? string.Empty,
            context.Command.SummaryHtml ?? string.Empty);
        CompositionRoot.EventStoreSession.Store(matchResult);

        return Task.CompletedTask;
    }

    public class Command : CommandBase
    {
        public Command(
            string rosterId,
            Parse4Result result,
            string? summaryText = null,
            string? summaryHtml = null)
        {
            RosterId = rosterId;
            Result = result;
            SummaryText = summaryText;
            SummaryHtml = summaryHtml;
        }

        public string RosterId { get; }
        public Parse4Result Result { get; }
        public string? SummaryText { get; }
        public string? SummaryHtml { get; }
    }
}

