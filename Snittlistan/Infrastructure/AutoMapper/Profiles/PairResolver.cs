using System.Linq;
using AutoMapper;
using Snittlistan.Models;
using Snittlistan.ViewModels;

namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
	public abstract class PairResolver : ValueResolver<Team, TeamViewModel.Pair>
	{
		public int Pair { get; set; }

		protected override TeamViewModel.Pair ResolveCore(Team source)
		{
			if (source.Series.Count() > Pair)
			{
				int[] indexes = GetScheme(Pair);

				return new TeamViewModel.Pair
				{
					Serie1 = source.Series.ElementAt(0).Tables.ElementAt(indexes[0]).MapTo<TeamViewModel.Serie>(),
					Serie2 = source.Series.ElementAt(1).Tables.ElementAt(indexes[1]).MapTo<TeamViewModel.Serie>(),
					Serie3 = source.Series.ElementAt(2).Tables.ElementAt(indexes[2]).MapTo<TeamViewModel.Serie>(),
					Serie4 = source.Series.ElementAt(3).Tables.ElementAt(indexes[3]).MapTo<TeamViewModel.Serie>()
				};
			}

			return new TeamViewModel.Pair();
		}

		protected abstract int[] GetScheme(int pair);
	}
}