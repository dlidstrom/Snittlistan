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

			Mapper.CreateMap<Game, MatchViewModel.Game>();
		}
	}
}