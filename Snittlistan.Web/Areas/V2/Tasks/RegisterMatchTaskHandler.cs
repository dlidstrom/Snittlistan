#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Elmah;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Areas.V2.Commands;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Indexes;
    using Snittlistan.Web.Infrastructure.Bits;
    using Snittlistan.Web.Models;

    public class RegisterMatchTaskHandler : TaskHandler<RegisterMatchTask>
    {
        public override async Task Handle(RegisterMatchTask task)
        {
            WebsiteConfig websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            Player[] players =
                DocumentSession.Query<Player, PlayerSearch>()
                    .ToArray()
                    .Where(x => x.PlayerItem?.LicNbr != null)
                    .ToArray();
            Roster pendingMatch = DocumentSession.Load<Roster>(task.RosterId);
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
                    Parse4Result parse4Result = parser.Parse4(bitsMatchResult, websiteConfig.ClubId);
                    if (parse4Result != null)
                    {
                        List<string> allPlayerIds = parse4Result.GetPlayerIds();
                        pendingMatch.SetPlayers(allPlayerIds);
                        ExecuteCommand(new RegisterMatch4Command(pendingMatch, parse4Result));
                    }
                }
                else
                {
                    ParseResult parseResult = parser.Parse(bitsMatchResult, websiteConfig.ClubId);
                    if (parseResult != null)
                    {
                        List<string> allPlayerIds = parseResult.GetPlayerIds();
                        pendingMatch.SetPlayers(allPlayerIds);
                        ExecuteCommand(new RegisterMatchCommand(pendingMatch, parseResult));
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
}
