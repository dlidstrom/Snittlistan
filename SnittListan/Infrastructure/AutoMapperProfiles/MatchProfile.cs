using System.Threading;
using AutoMapper;
using SnittListan.Models;
using SnittListan.ViewModels;

namespace SnittListan.Infrastructure
{
	public class MatchProfile : Profile
	{
		protected override void Configure()
		{
			Mapper.CreateMap<Match, MatchViewModel.MatchInfo>()
				.ForMember(x => x.Date, o => o.MapFrom(y => y.Date))
				.ForMember(x => x.HomeTeam, o => o.MapFrom(y => y.HomeTeam.Name))
				.ForMember(x => x.HomeTeamScore, o => o.MapFrom(y => y.HomeTeam.Score))
				.ForMember(x => x.AwayTeam, o => o.MapFrom(y => y.AwayTeam.Name))
				.ForMember(x => x.AwayTeamScore, o => o.MapFrom(y => y.AwayTeam.Score));

			Mapper.CreateMap<Match, MatchInfoViewModel>();
			Mapper.CreateMap<Game, MatchViewModel.Game>();
		}
	}
}