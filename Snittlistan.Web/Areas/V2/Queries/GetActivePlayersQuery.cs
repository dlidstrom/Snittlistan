namespace Snittlistan.Web.Areas.V2.Queries
{
    using System.Linq;
    using Raven.Client;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Indexes;
    using Snittlistan.Web.Infrastructure;

    public class GetActivePlayersQuery : IQuery<Player[]>
    {
        public Player[] Execute(IDocumentSession session)
        {
            var players = session.Query<Player, PlayerSearch>()
                                 .Where(x => x.PlayerStatus == Player.Status.Active)
                                 .ToArray();
            return players;
        }
    }
}