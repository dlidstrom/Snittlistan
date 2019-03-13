namespace Snittlistan.Web.Areas.V2.Indexes
{
    using System.Linq;
    using Raven.Client.Indexes;
    using Snittlistan.Web.Areas.V2.Domain;

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
}