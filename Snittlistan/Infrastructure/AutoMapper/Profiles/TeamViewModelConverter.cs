using System.Collections.Generic;
using AutoMapper;
using Snittlistan.Models;
using Snittlistan.ViewModels;

namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
	public class TeamViewModelConverter : ITypeConverter<TeamViewModel, Team>
	{
		public Team Convert(ResolutionContext context)
		{
			var vm = (TeamViewModel)context.SourceValue;
			return new Team(
				vm.Name,
				vm.Score,
				new List<Serie>
				{
					vm.Serie1.MapTo<Serie>(),
					vm.Serie2.MapTo<Serie>(),
					vm.Serie3.MapTo<Serie>(),
					vm.Serie4.MapTo<Serie>()
				});
		}
	}
}