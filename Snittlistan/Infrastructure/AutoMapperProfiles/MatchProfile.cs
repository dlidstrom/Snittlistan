using AutoMapper;
using Snittlistan.Models;
using Snittlistan.ViewModels;

namespace Snittlistan.Infrastructure
{
	public class MatchProfile : Profile
	{
		protected override void Configure()
		{
			Mapper.CreateMap<Match, MatchViewModel.MatchDetails>()
				.ForMember(x => x.Id, o => o.MapFrom(y => y.Id))
				.ForMember(x => x.Date, o => o.MapFrom(y => y.Date))
				.ForMember(x => x.BitsMatchId, o => o.MapFrom(y => y.BitsMatchId));

			// model -> viewmodel
			Mapper.CreateMap<Team, TeamViewModel>()
				.ForMember(t => t.Id, o => o.Ignore());
			Mapper.CreateMap<Serie, TeamViewModel.Serie>();
			Mapper.CreateMap<Table, TeamViewModel.Table>();
			Mapper.CreateMap<Game, TeamViewModel.Game>();

			// viewmodel -> model
			Mapper.CreateMap<TeamViewModel, Team>()
				.ConstructUsing(vm => new Team(vm.Name, vm.Score));
			Mapper.CreateMap<TeamViewModel.Serie, Serie>();
			Mapper.CreateMap<TeamViewModel.Table, Table>();
			Mapper.CreateMap<TeamViewModel.Game, Game>();
		}
	}
}