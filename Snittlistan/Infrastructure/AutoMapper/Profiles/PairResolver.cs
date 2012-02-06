namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
    using System.Linq;
    using AutoMapper;
    using Snittlistan.Models;
    using Snittlistan.ViewModels.Match;

    public class PairResolver : global::AutoMapper.ValueResolver<Team8x4, Team8x4ViewModel.Pair>
    {
        public int Pair { get; set; }

        protected override Team8x4ViewModel.Pair ResolveCore(Team8x4 source)
        {
            if (source.Series.Count() > Pair)
            {
                return new Team8x4ViewModel.Pair
                {
                    Serie1 = source.TableAt(0, Pair).MapTo<Team8x4ViewModel.Serie>(),
                    Serie2 = source.TableAt(1, Pair).MapTo<Team8x4ViewModel.Serie>(),
                    Serie3 = source.TableAt(2, Pair).MapTo<Team8x4ViewModel.Serie>(),
                    Serie4 = source.TableAt(3, Pair).MapTo<Team8x4ViewModel.Serie>()
                };
            }

            return new Team8x4ViewModel.Pair();
        }
    }
}