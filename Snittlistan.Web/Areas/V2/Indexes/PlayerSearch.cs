#nullable enable

using Raven.Client.Documents.Indexes;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.Indexes;

public class PlayerSearch : AbstractIndexCreationTask<Player>
{
    public PlayerSearch()
    {
        Map = players => from player in players
                         select new
                         {
                             player.Name,
                             player.Email,
                             player.PlayerStatus
                         };
    }
}
