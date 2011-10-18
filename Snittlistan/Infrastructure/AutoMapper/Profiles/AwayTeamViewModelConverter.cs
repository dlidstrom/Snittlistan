using System.Collections.Generic;
using AutoMapper;
using Snittlistan.Models;
using Snittlistan.ViewModels;

namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
	/// <summary>
	/// H1-B1 H2-B2 H3-B3 H4-B4
	/// H3-B4 H4-B3 H1-B2 H2-B1
	/// H4-B2 H3-B1 H2-B4 H1-B3
	/// H2-B3 H1-B4 H4-B1 H3-B2.
	/// </summary>
	public class AwayTeamViewModelConverter : ITypeConverter<AwayTeamViewModel, Team>
	{
		public Team Convert(ResolutionContext context)
		{
			var vm = (TeamViewModel)context.SourceValue;
			return new Team(
				vm.Name,
				vm.Score,
				new List<Serie>
			    {
					new Serie(new List<Table>
					{
						vm.Pair1.Serie1.MapTo<Table>(),
						vm.Pair2.Serie1.MapTo<Table>(),
						vm.Pair3.Serie1.MapTo<Table>(),
						vm.Pair4.Serie1.MapTo<Table>()
					}),
					new Serie(new List<Table>
					{
						vm.Pair4.Serie2.MapTo<Table>(),
						vm.Pair3.Serie2.MapTo<Table>(),
						vm.Pair2.Serie2.MapTo<Table>(),
						vm.Pair1.Serie2.MapTo<Table>()
					}),
					new Serie(new List<Table>
					{
						vm.Pair2.Serie3.MapTo<Table>(),
						vm.Pair1.Serie3.MapTo<Table>(),
						vm.Pair4.Serie3.MapTo<Table>(),
						vm.Pair3.Serie3.MapTo<Table>()
					}),
					new Serie(new List<Table>
					{
						vm.Pair3.Serie4.MapTo<Table>(),
						vm.Pair4.Serie4.MapTo<Table>(),
						vm.Pair1.Serie4.MapTo<Table>(),
						vm.Pair2.Serie4.MapTo<Table>()
					}),
			    });
		}
	}
}