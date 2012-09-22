namespace Snittlistan.Web.Infrastructure.AutoMapper.Profiles
{
    using System.Linq;

    using Snittlistan.Web.Models;
    using Snittlistan.Web.ViewModels.Match;

    public class PlayerResolver : global::AutoMapper.ValueResolver<Team4x4, Team4x4ViewModel.Player>
    {
        public int Player { get; set; }

        protected override Team4x4ViewModel.Player ResolveCore(Team4x4 source)
        {
            if (source.Series.Count() == 4)
            {
                return new Team4x4ViewModel.Player
                {
                    Game1 = source.Series.ElementAt(0).Games.ElementAt(this.Player).MapTo<Team4x4ViewModel.Game>(),
                    Game2 = source.Series.ElementAt(1).Games.ElementAt(this.Player).MapTo<Team4x4ViewModel.Game>(),
                    Game3 = source.Series.ElementAt(2).Games.ElementAt(this.Player).MapTo<Team4x4ViewModel.Game>(),
                    Game4 = source.Series.ElementAt(3).Games.ElementAt(this.Player).MapTo<Team4x4ViewModel.Game>()
                };
            }

            return new Team4x4ViewModel.Player();
        }
    }
}