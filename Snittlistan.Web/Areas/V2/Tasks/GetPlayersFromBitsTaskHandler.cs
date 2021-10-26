#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Indexes;
    using Snittlistan.Web.Infrastructure.Bits.Contracts;
    using Snittlistan.Web.Models;

    public class GetPlayersFromBitsTaskHandler : TaskHandler<GetPlayersFromBitsTask>
    {
        public override async Task Handle(MessageContext<GetPlayersFromBitsTask> task)
        {
            WebsiteConfig websiteConfig = DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
            PlayerResult playersResult = await BitsClient.GetPlayers(websiteConfig.ClubId);
            Player[] players = DocumentSession.Query<Player, PlayerSearch>()
                .ToArray();

            // update existing players by matching on license number
            Dictionary<string, PlayerItem> playersByLicense = playersResult.Data.ToDictionary(x => x.LicNbr);
            foreach (Player player in players.Where(x => x.PlayerItem != null))
            {
                if (playersByLicense.TryGetValue(player.PlayerItem.LicNbr, out PlayerItem playerItem))
                {
                    player.PlayerItem = playerItem;
                    _ = playersByLicense.Remove(player.PlayerItem.LicNbr);
                    Log.Info($"Updating player with existing PlayerItem: {player.PlayerItem.LicNbr}");
                }
                else
                {
                    Log.Info($"Player with {player.PlayerItem.LicNbr} not found from BITS");
                }
            }

            // add missing players, i.e. what is left from first step
            // try first to match on name, update those, add the rest
            Dictionary<string, Player> playerNamesWithoutPlayerItem = players.Where(x => x.PlayerItem == null).ToDictionary(x => x.Name);
            foreach (PlayerItem playerItem in playersByLicense.Values)
            {
                // look for name
                string nameFromBits = $"{playerItem.FirstName} {playerItem.SurName}";
                if (playerNamesWithoutPlayerItem.TryGetValue(nameFromBits, out Player player))
                {
                    player.PlayerItem = playerItem;
                    Log.Info($"Updating player with missing PlayerItem: {nameFromBits}");
                }
                else
                {
                    // create new
                    Player newPlayer = new(
                        $"{playerItem.FirstName} {playerItem.SurName}",
                        playerItem.Email,
                        playerItem.Inactive ? Player.Status.Inactive : Player.Status.Active,
                        playerItem.GetPersonalNumber(),
                        string.Empty,
                        new string[0])
                    {
                        PlayerItem = playerItem
                    };
                    Log.Info($"Created player {playerItem.FirstName} {playerItem.SurName}");
                    DocumentSession.Store(newPlayer);
                }
            }
        }
    }
}
