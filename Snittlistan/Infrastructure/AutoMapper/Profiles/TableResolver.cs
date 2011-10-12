using System.Linq;
using AutoMapper;
using Snittlistan.Models;
using Snittlistan.ViewModels;

namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
	public class TableResolver : ValueResolver<Serie, TeamViewModel.Table>
	{
		public int Table { get; set; }
		protected override TeamViewModel.Table ResolveCore(Serie source)
		{
			if (source.Tables.Count() > Table)
				return source.Tables.ElementAt(Table).MapTo<TeamViewModel.Table>();
			return new TeamViewModel.Table();
		}
	}
}
