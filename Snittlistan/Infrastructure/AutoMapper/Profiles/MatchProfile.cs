using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Snittlistan.Models;
using Snittlistan.ViewModels;

namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
	public class MatchProfile : Profile
	{
		protected override void Configure()
		{
			// model -> viewmodel
			Mapper.CreateMap<Match, MatchViewModel.MatchDetails>();
			Mapper.CreateMap<Team, TeamSummaryViewModel>();
			Mapper.CreateMap<Team, TeamViewModel>()
				.ForMember(vm => vm.Serie1, o => o.ResolveUsing(new SerieResolver { Serie = 0 }))
				.ForMember(vm => vm.Serie2, o => o.ResolveUsing(new SerieResolver { Serie = 1 }))
				.ForMember(vm => vm.Serie3, o => o.ResolveUsing(new SerieResolver { Serie = 2 }))
				.ForMember(vm => vm.Serie4, o => o.ResolveUsing(new SerieResolver { Serie = 3 }));
			Mapper.CreateMap<Serie, TeamViewModel.Serie>()
				.ForMember(vm => vm.Table1, o => o.ResolveUsing(new TableResolver { Table = 0 }))
				.ForMember(vm => vm.Table2, o => o.ResolveUsing(new TableResolver { Table = 1 }))
				.ForMember(vm => vm.Table3, o => o.ResolveUsing(new TableResolver { Table = 2 }))
				.ForMember(vm => vm.Table4, o => o.ResolveUsing(new TableResolver { Table = 3 }));
			Mapper.CreateMap<Table, TeamViewModel.Table>()
				.ForMember(vm => vm.Game1, o => o.MapFrom(m => m.Game1))
				.ForMember(vm => vm.Game2, o => o.MapFrom(m => m.Game2));
			Mapper.CreateMap<Game, TeamViewModel.Game>();

			// viewmodel -> model
			Mapper.CreateMap<TeamViewModel, Team>().ConvertUsing<TeamViewModelConverter>();
			Mapper.CreateMap<TeamViewModel.Serie, Serie>().ConvertUsing<TeamViewModelSerieConverter>();
			Mapper.CreateMap<TeamViewModel.Table, Table>();
			Mapper.CreateMap<TeamViewModel.Game, Game>();
		}
	}
}