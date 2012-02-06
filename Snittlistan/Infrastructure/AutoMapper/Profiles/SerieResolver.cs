namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
    using System.Linq;
    using AutoMapper;
    using Snittlistan.Models;
    using Snittlistan.ViewModels.Match;

    public class SerieResolver : global::AutoMapper.ValueResolver<Team4x4, Team4x4ViewModel.Serie>
    {
        public int Serie { get; set; }

        protected override Team4x4ViewModel.Serie ResolveCore(Team4x4 source)
        {
            if (source.Series.Count() > Serie)
            {
                return new Team4x4ViewModel.Serie
                {
                    Game1 = source.Series.ElementAt(Serie).Games.ElementAt(0).MapTo<Team4x4ViewModel.Game>(),
                    Game2 = source.Series.ElementAt(Serie).Games.ElementAt(1).MapTo<Team4x4ViewModel.Game>(),
                    Game3 = source.Series.ElementAt(Serie).Games.ElementAt(2).MapTo<Team4x4ViewModel.Game>(),
                    Game4 = source.Series.ElementAt(Serie).Games.ElementAt(3).MapTo<Team4x4ViewModel.Game>()
                };
            }

            return new Team4x4ViewModel.Serie();
        }
    }
}