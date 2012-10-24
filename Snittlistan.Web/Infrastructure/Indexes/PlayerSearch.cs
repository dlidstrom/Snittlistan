namespace Snittlistan.Web.Infrastructure.Indexes
{
    using System.Linq;

    using Raven.Client.Indexes;

    using Snittlistan.Web.Areas.V2.Models;

    public class PlayerSearch : AbstractIndexCreationTask<Player>
    {
        public PlayerSearch()
        {
            this.Map = players => from player in players
                                  select new
                                  {
                                      player.Name,
                                      player.Email,
                                      player.IsSupporter
                                  };
        }
    }
}