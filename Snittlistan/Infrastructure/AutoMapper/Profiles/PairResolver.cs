using System.Linq;
using AutoMapper;
using Snittlistan.Models;
using Snittlistan.ViewModels;

namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
	public class PairResolver : ValueResolver<Team, TeamViewModel.Pair>
	{
		public int Pair { get; set; }

		protected override TeamViewModel.Pair ResolveCore(Team source)
		{
			if (source.Series.Count() > Pair)
			{
				return new TeamViewModel.Pair
				{
					Serie1 = source.TableAt(0, Pair).MapTo<TeamViewModel.Serie>(),
					Serie2 = source.TableAt(1, Pair).MapTo<TeamViewModel.Serie>(),
					Serie3 = source.TableAt(2, Pair).MapTo<TeamViewModel.Serie>(),
					Serie4 = source.TableAt(3, Pair).MapTo<TeamViewModel.Serie>()
				};
			}

			return new TeamViewModel.Pair();
		}
	}
}