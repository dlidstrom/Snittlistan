using System.Linq;
using AutoMapper;
using Snittlistan.Models;
using Snittlistan.ViewModels;

namespace Snittlistan.Infrastructure
{
	public class SerieResolver : ValueResolver<Team, TeamViewModel.Serie>
	{
		public int Serie { get; set; }
		protected override TeamViewModel.Serie ResolveCore(Team source)
		{
			if (source.Series.Count > Serie)
				return source.Series.ElementAt(Serie).MapTo<TeamViewModel.Serie>();
			return new TeamViewModel.Serie();
		}
	}
}