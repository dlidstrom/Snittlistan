using Elmah;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Commands;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Bits;
using Snittlistan.Web.Models;

#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks;
public class RegisterMatchTaskHandler : TaskHandler<RegisterMatchTask>
{
    public override async Task Handle(MessageContext<RegisterMatchTask> context)
    {
        WebsiteConfig websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
        Player[] players =
            DocumentSession.Query<Player, PlayerSearch>()
                .ToArray()
                .Where(x => x.PlayerItem?.LicNbr != null)
                .ToArray();
        Roster pendingMatch = DocumentSession.Load<Roster>(context.Task.RosterId);
        try
        {
            BitsParser parser = new(players);
            BitsMatchResult bitsMatchResult = await BitsClient.GetBitsMatchResult(pendingMatch.BitsMatchId);
            if (bitsMatchResult.HeadInfo.MatchFinished == false)
            {
                Log.Info($"Match {pendingMatch.BitsMatchId} not yet finished");
                return;
            }

            if (pendingMatch.IsFourPlayer)
            {
                Parse4Result? parse4Result = parser.Parse4(bitsMatchResult, websiteConfig.ClubId);
                if (parse4Result != null)
                {
                    if (parse4Result.Series.Length != 4 && parse4Result.Turn <= 20)
                    {
                        Log.Info($"detected unfinished match: {parse4Result.Series.Length} series have been registered");
                        return;
                    }

                    List<string> allPlayerIds = parse4Result.GetPlayerIds();
                    pendingMatch.SetPlayers(allPlayerIds);
                    await context.ExecuteCommand(
                        new RegisterMatch4Command(pendingMatch, parse4Result),
                        DocumentSession,
                        EventStoreSession);
                }
            }
            else
            {
                ParseResult? parseResult = parser.Parse(bitsMatchResult, websiteConfig.ClubId);
                if (parseResult != null)
                {
                    if (parseResult.Series.Length != 4 && parseResult.Turn <= 20)
                    {
                        Log.Info($"detected unfinished match: {parseResult.Series.Length} series have been registered");
                        return;
                    }

                    List<string> allPlayerIds = parseResult.GetPlayerIds();
                    pendingMatch.SetPlayers(allPlayerIds);
                    await context.ExecuteCommand(
                        new RegisterMatchCommand(pendingMatch, parseResult),
                        DocumentSession,
                        EventStoreSession);
                }
            }
        }
        catch (Exception e)
        {
            ErrorSignal
                .FromCurrentContext()
                .Raise(new Exception($"Unable to auto register match {pendingMatch.Id} ({pendingMatch.BitsMatchId})", e));
            Log.Warn(e);
        }
    }
}
