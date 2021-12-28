using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Bits;
using Snittlistan.Web.Infrastructure.Bits.Contracts;
using Snittlistan.Web.Models;

#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks;
public class VerifyMatchTaskHandler : TaskHandler<VerifyMatchTask>
{
    public override async Task Handle(MessageContext<VerifyMatchTask> context)
    {
        Roster roster = DocumentSession.Load<Roster>(context.Task.RosterId);
        if (roster.IsVerified && context.Task.Force == false)
        {
            return;
        }

        WebsiteConfig websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
        HeadInfo result = await BitsClient.GetHeadInfo(roster.BitsMatchId);
        ParseHeaderResult header = BitsParser.ParseHeader(result, websiteConfig.ClubId);

        // chance to update roster values
        Roster.Update update = new(
            Roster.ChangeType.VerifyMatchMessage,
            "system")
        {
            OilPattern = header.OilPattern,
            Date = header.Date,
            Opponent = header.Opponent,
            Location = header.Location
        };
        if (roster.MatchResultId != null)
        {
            // update match result values
            BitsMatchResult bitsMatchResult = await BitsClient.GetBitsMatchResult(roster.BitsMatchId);
            Player[] players = DocumentSession.Query<Player, PlayerSearch>()
                .ToArray()
                .Where(x => x.PlayerItem?.LicNbr != null)
                .ToArray();
            BitsParser parser = new(players);
            if (roster.IsFourPlayer)
            {
                MatchResult4? matchResult = EventStoreSession.Load<MatchResult4>(roster.MatchResultId);
                Parse4Result? parseResult = parser.Parse4(bitsMatchResult, websiteConfig.ClubId);
                update.Players = parseResult!.GetPlayerIds();
                bool isVerified = matchResult!.Update(
                    context.PublishMessage,
                    roster,
                    parseResult.TeamScore,
                    parseResult.OpponentScore,
                    roster.BitsMatchId,
                    parseResult.CreateMatchSeries(),
                    players);
                update.IsVerified = isVerified;
            }
            else
            {
                MatchResult? matchResult = EventStoreSession.Load<MatchResult>(roster.MatchResultId);
                ParseResult? parseResult = parser.Parse(bitsMatchResult, websiteConfig.ClubId);
                update.Players = parseResult!.GetPlayerIds();
                Dictionary<string, ResultForPlayerIndex.Result> resultsForPlayer =
                    DocumentSession.Query<ResultForPlayerIndex.Result, ResultForPlayerIndex>()
                    .Where(x => x.Season == roster.Season)
                    .ToArray()
                    .ToDictionary(x => x.PlayerId);
                MatchSerie[] matchSeries = parseResult.CreateMatchSeries();
                bool isVerified = matchResult!.Update(
                    context.PublishMessage,
                    roster,
                    parseResult.TeamScore,
                    parseResult.OpponentScore,
                    matchSeries,
                    parseResult.OpponentSeries,
                    players,
                    resultsForPlayer);
                update.IsVerified = isVerified;
            }
        }

        roster.UpdateWith(context.CorrelationId, update);
    }
}
