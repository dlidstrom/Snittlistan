#nullable enable

using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Bits.Contracts;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Commands;

public class GetPlayersFromBitsCommandHandler : CommandHandler<GetPlayersFromBitsCommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
    {
        WebsiteConfig websiteConfig = CompositionRoot.DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
        PlayerResult playersResult = await CompositionRoot.BitsClient.GetPlayers(websiteConfig.ClubId);
        Player[] players = CompositionRoot.DocumentSession.Query<Player, PlayerSearch>()
            .ToArray();

        // update existing players by matching on license number
        Dictionary<string, PlayerItem> playersByLicense = playersResult.Data.ToDictionary(x => x.LicNbr!);
        foreach (Player player in players.Where(x => x.PlayerItem != null))
        {
            if (playersByLicense.TryGetValue(player.PlayerItem!.LicNbr!, out PlayerItem playerItem))
            {
                player.PlayerItem = playerItem;
                _ = playersByLicense.Remove(player.PlayerItem.LicNbr!);
                Logger.InfoFormat(
                    "Updating player with existing PlayerItem {licNbr}",
                    player.PlayerItem.LicNbr);
            }
            else
            {
                Logger.InfoFormat(
                    "Player with {licNbr} not found from BITS",
                    player.PlayerItem.LicNbr);
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
                Logger.InfoFormat(
                    "Updating player with missing PlayerItem: {nameFromBits}",
                    nameFromBits);
            }
            else
            {
                // create new
                Player newPlayer = new(
                    $"{playerItem.FirstName} {playerItem.SurName}",
                    playerItem.Email!,
                    playerItem.Inactive ? Player.Status.Inactive : Player.Status.Active,
                    playerItem.GetPersonalNumber(),
                    string.Empty,
                    new string[0])
                {
                    PlayerItem = playerItem
                };
                Logger.InfoFormat(
                    "Created player {firstName} {surName}",
                    playerItem.FirstName,
                    playerItem.SurName);
                CompositionRoot.DocumentSession.Store(newPlayer);
            }
        }
    }

    public record Command();
}
