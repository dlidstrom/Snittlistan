using System;
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
				switch (Pair)
				{
					case 0:
						return new TeamViewModel.Pair
						{
							Serie1 = source.Series.ElementAt(0).Tables.ElementAt(0).MapTo<TeamViewModel.Serie>(),
							Serie2 = source.Series.ElementAt(1).Tables.ElementAt(2).MapTo<TeamViewModel.Serie>(),
							Serie3 = source.Series.ElementAt(2).Tables.ElementAt(3).MapTo<TeamViewModel.Serie>(),
							Serie4 = source.Series.ElementAt(3).Tables.ElementAt(1).MapTo<TeamViewModel.Serie>()
						};
					case 1:
						return new TeamViewModel.Pair
						{
							Serie1 = source.Series.ElementAt(0).Tables.ElementAt(1).MapTo<TeamViewModel.Serie>(),
							Serie2 = source.Series.ElementAt(1).Tables.ElementAt(3).MapTo<TeamViewModel.Serie>(),
							Serie3 = source.Series.ElementAt(2).Tables.ElementAt(2).MapTo<TeamViewModel.Serie>(),
							Serie4 = source.Series.ElementAt(3).Tables.ElementAt(0).MapTo<TeamViewModel.Serie>()
						};
					case 2:
						return new TeamViewModel.Pair
						{
							Serie1 = source.Series.ElementAt(0).Tables.ElementAt(2).MapTo<TeamViewModel.Serie>(),
							Serie2 = source.Series.ElementAt(1).Tables.ElementAt(0).MapTo<TeamViewModel.Serie>(),
							Serie3 = source.Series.ElementAt(2).Tables.ElementAt(1).MapTo<TeamViewModel.Serie>(),
							Serie4 = source.Series.ElementAt(3).Tables.ElementAt(3).MapTo<TeamViewModel.Serie>()
						};
					case 3:
						return new TeamViewModel.Pair
						{
							Serie1 = source.Series.ElementAt(0).Tables.ElementAt(3).MapTo<TeamViewModel.Serie>(),
							Serie2 = source.Series.ElementAt(1).Tables.ElementAt(1).MapTo<TeamViewModel.Serie>(),
							Serie3 = source.Series.ElementAt(2).Tables.ElementAt(0).MapTo<TeamViewModel.Serie>(),
							Serie4 = source.Series.ElementAt(3).Tables.ElementAt(2).MapTo<TeamViewModel.Serie>()
						};
					default:
						throw new ArgumentException("Pair");
				}
			}

			return new TeamViewModel.Pair();
		}
	}
}