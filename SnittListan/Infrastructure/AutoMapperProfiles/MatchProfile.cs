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
			Mapper.CreateMap<Match, MatchViewModel>()
				.ForMember(x => x.Date, o => o.MapFrom(y => y.Date))
				.ForMember(x => x.HomeTeam, o => o.MapFrom(y => y.HomeTeam))
				.ForMember(
					x => x.HomeTeamLaneScore,
					o => o.MapFrom(y =>
					{
						return y.HomeGame ? y.LaneScoreForTeam() : y.OppTeamLaneScore;
					}))
				.ForMember(x => x.OppTeam, o => o.MapFrom(y => y.OppTeam))
				.ForMember(
					x => x.OppTeamLaneScore,
					o => o.MapFrom(y =>
					{
						return y.HomeGame ? y.OppTeamLaneScore : y.LaneScoreForTeam();
					}))
				.ForMember(x => x.Games, o => o.MapFrom(y => y.Games));

			Mapper.CreateMap<Game, MatchViewModel.Game>();
		}
	}
}