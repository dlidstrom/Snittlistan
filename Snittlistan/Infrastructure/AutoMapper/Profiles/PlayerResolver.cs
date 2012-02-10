namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
    using System.Linq;
    using AutoMapper;
    using Snittlistan.Models;
    using Snittlistan.ViewModels.Match;

    public class PlayerResolver : global::AutoMapper.ValueResolver<Team4x4, Team4x4ViewModel.Player>
    {
        public int Player { get; set; }

        protected override Team4x4ViewModel.Player ResolveCore(Team4x4 source)
        {
            if (source.Series.Count() == 4)
            {
                return new Team4x4ViewModel.Player
                {
                    Game1 = source.Series.ElementAt(0).Games.ElementAt(Player).MapTo<Team4x4ViewModel.Game>(),
                    Game2 = source.Series.ElementAt(1).Games.ElementAt(Player).MapTo<Team4x4ViewModel.Game>(),
                    Game3 = source.Series.ElementAt(2).Games.ElementAt(Player).MapTo<Team4x4ViewModel.Game>(),
                    Game4 = source.Series.ElementAt(3).Games.ElementAt(Player).MapTo<Team4x4ViewModel.Game>()
                };
            }

            return new Team4x4ViewModel.Player();
        }
    }
}